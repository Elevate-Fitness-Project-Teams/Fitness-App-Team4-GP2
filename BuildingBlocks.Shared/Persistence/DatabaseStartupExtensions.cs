using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Shared.Persistence
{
    /// <summary>
    /// Applies pending EF Core migrations at startup so a service can create its own
    /// schema on a database it has never seen before - which is exactly the Docker
    /// case, where the SQL Server container starts empty on the first run.
    ///
    /// Seed data rides along: every catalog seed in this solution is declared with
    /// HasData (Nutrition, Workout, FCE, Notification), so it is baked into a
    /// migration and inserted by the same Migrate() call. There is no separate seeding
    /// step to run, and re-running is safe - EF only applies migrations the
    /// __EFMigrationsHistory table does not already list.
    /// </summary>
    public static class DatabaseStartupExtensions
    {
        /// <summary>
        /// Migrates <typeparamref name="TContext"/> unless "Database:AutoMigrate" is false.
        ///
        /// Retries because compose only guarantees the SQL Server container is healthy,
        /// not that it is accepting logins - and on a cold start SQL Server can still be
        /// recovering databases when the first connection arrives.
        /// </summary>
        public static IHost MigrateDatabase<TContext>(
            this IHost host,
            int retries = 12,
            int delaySeconds = 5)
            where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var sp = scope.ServiceProvider;

            var configuration = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetRequiredService<ILoggerFactory>()
                           .CreateLogger(typeof(DatabaseStartupExtensions));

            if (!configuration.GetValue("Database:AutoMigrate", true))
            {
                logger.LogInformation(
                    "Database:AutoMigrate is false - skipping migrations for {Context}.",
                    typeof(TContext).Name);
                return host;
            }

            var db = sp.GetRequiredService<TContext>();

            for (var attempt = 1; ; attempt++)
            {
                try
                {
                    var pending = db.Database.GetPendingMigrations().ToList();
                    if (pending.Count == 0)
                    {
                        logger.LogInformation("{Context}: database is up to date.", typeof(TContext).Name);
                        return host;
                    }

                    logger.LogInformation(
                        "{Context}: applying {Count} migration(s): {Migrations}",
                        typeof(TContext).Name, pending.Count, string.Join(", ", pending));

                    db.Database.Migrate();

                    logger.LogInformation("{Context}: migrations applied.", typeof(TContext).Name);
                    return host;
                }
                catch (Exception ex) when (attempt < retries)
                {
                    logger.LogWarning(
                        ex,
                        "{Context}: database not reachable (attempt {Attempt}/{Retries}), retrying in {Delay}s.",
                        typeof(TContext).Name, attempt, retries, delaySeconds);

                    Thread.Sleep(TimeSpan.FromSeconds(delaySeconds));
                }
            }
        }
    }
}
