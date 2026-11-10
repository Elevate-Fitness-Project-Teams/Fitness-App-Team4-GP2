# Database & Docker Compose

Every service owns its own database on a single SQL Server container defined in
`docker-compose.yml`. Nothing is created by hand: each service migrates and seeds
its own database the moment it starts.

---

## 1. Running it

```bash
docker compose up -d --build
```

Order of events on a cold start:

1. `sqlserver` starts and its healthcheck (`sqlcmd -Q 'SELECT 1'`) begins probing.
2. Every service has `depends_on: sqlserver: condition: service_healthy`, so nothing
   connects before SQL Server accepts logins.
3. Each service calls `app.MigrateDatabase<TContext>()` before serving traffic —
   it creates the database if missing, applies pending migrations, and inserts the
   `HasData` seed rows that ship inside those migrations.

The data lives in the named volume `elevate-sqlserver-data`. `docker compose down`
keeps it; only `docker compose down -v` wipes it.

### Databases and ports

| Service | Database | HTTP | HTTPS |
|---|---|---|---|
| AuthService | `ElevateAuthDb` | 5000 | 5001 |
| FCEService | `ElevateFceDb` | 5002 | 5003 |
| NotificationService | `ElevateNotificationDb` | 5004 | 5005 |
| NutritionService | `ElevateNutritionDb` | 5006 | 5007 |
| ProfileService | `ElevateProfileDb` | 5008 | 5009 |
| ProgressService | `ElevateProgressDb` | 5010 | 5011 |
| SmartCoachService | `ElevateSmartCoachDb` | 5012 | 5013 |
| SubscriptionService | `ElevateSubscriptionDb` | 5014 | 5015 |
| WorkoutService | `ElevateWorkoutDb` | 5016 | 5017 |

SQL Server itself is published on `localhost,1433` — user `sa`, password
`Elevate_Str0ng_Pass1` (override by setting `MSSQL_SA_PASSWORD` in a root `.env`
file; the compose files read it in both places).

RabbitMQ is **not** in compose — services still point at `host.docker.internal`,
so keep running it on Windows as before.

---

## 2. Seed data

All seeding goes through EF Core `HasData`, so the rows are baked into a migration
and are applied by the same `Migrate()` call — there is no separate seeder to run,
and re-running is safe.

| Service | Seed class | Migration | Rows |
|---|---|---|---|
| NutritionService | `NutritionCatalogSeed` | `SeedNutritionCatalog` | 16 meals, 4 plans, 18 plan items |
| WorkoutService | `WorkoutCatalogSeed` | `SeedWorkoutCatalog` | 25 exercises + plans/workouts |
| FCEService | `FitnessPlanConfigSeed` | `SeedFitnessPlanConfig` | 15 plan configs |

NotificationService has a migration named `seed data`, but its `Up`/`Down` are empty —
it seeds nothing. Notifications are produced at runtime, so there is no catalog to
seed; the migration is a leftover and can stay as-is (removing it would break the
history on databases that already recorded it).

**To change seed data:** edit the seed class, then generate a migration (§3). EF
diffs the `HasData` rows and emits the matching `InsertData` / `UpdateData` /
`DeleteData` statements. Never edit an already-applied seed migration in place —
add a new one, otherwise boxes that already ran it will silently disagree.

Keys in seed data are fixed literals on purpose. If you change a key, EF deletes
the old row and inserts a new one.

---

## 3. Tracking database changes

### The schema history is the migrations folder

A migration **is** the change record. Every schema change is a timestamped, code-reviewed
file in git, and each database tracks what it has applied in its own
`__EFMigrationsHistory` table.

Add a change:

```bash
dotnet ef migrations add AddSomethingUseful --project NutritionService --startup-project NutritionService
```

(For AuthService, FCEService, ProfileService and WorkoutService the migrations live under
`Infrastructure/Persistence/Migrations`; add `--output-dir Infrastructure/Persistence/Migrations`.)

You do **not** need to run `dotnet ef database update` for Docker — restarting the
service applies it. To apply it against a local/host database instead:

```bash
dotnet ef database update --project NutritionService --startup-project NutritionService
```

### See what a service is about to do

```bash
dotnet ef migrations list --project NutritionService --startup-project NutritionService
```

Pending migrations are marked. To read the exact SQL before it runs anywhere:

```bash
dotnet ef migrations script --idempotent --project NutritionService --startup-project NutritionService
```

### See what a service actually did

The startup migrator logs every run:

```bash
docker compose logs nutritionservice | grep -E "applying|up to date"
```

You get either `applying 3 migration(s): ...` (with the full SQL underneath, since
EF logs commands at Information level in Development) or `database is up to date.`

### Query the history directly

```bash
docker compose exec sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Elevate_Str0ng_Pass1" -C -Q "SELECT * FROM ElevateNutritionDb.dbo.__EFMigrationsHistory ORDER BY MigrationId"
```

That table is the ground truth for "which version is this database on" — compare it
against `dotnet ef migrations list` to spot a box that is behind.

### Browse data and diff schema by eye

Connect SSMS or Azure Data Studio to `localhost,1433` (`sa` / `Elevate_Str0ng_Pass1`,
Trust server certificate = yes). Useful when you want to confirm seed rows landed or
inspect what a handler wrote.

### Detect model drift

`ModelSnapshot.cs` in each Migrations folder is the model as of the last migration.
If you changed an entity but forgot the migration, `dotnet ef migrations add` will
produce a non-empty file — an empty one means the model and the snapshot agree.
Committing entity changes without the matching migration is the single most common
way these databases fall out of sync.

---

## 4. Turning auto-migration off

`app.MigrateDatabase<TContext>()` respects configuration:

```json
{ "Database": { "AutoMigrate": false } }
```

or the env var `Database__AutoMigrate=false`. Off means the service starts without
touching the schema — use it if you ever want migrations gated behind a deliberate
deployment step rather than a container restart.

---

## 5. Resetting

```bash
docker compose down -v
docker compose up -d --build
```

`-v` drops the volume, so every database is recreated and reseeded from scratch.
To reset a single service, drop just its database and restart that container:

```bash
docker compose exec sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Elevate_Str0ng_Pass1" -C -Q "DROP DATABASE ElevateNutritionDb"
```

```bash
docker compose restart nutritionservice
```
