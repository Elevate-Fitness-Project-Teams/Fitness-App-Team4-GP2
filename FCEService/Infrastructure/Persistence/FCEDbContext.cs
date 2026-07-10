using FCEService.Domain.Entities;
using FCEService.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace FCEService.Infrastructure.Persistence
{
    public class FCEDbContext : DbContext
    {
        public FCEDbContext(DbContextOptions<FCEDbContext> options) : base(options)
        {
        }

        public DbSet<UserFitnessStat> UserFitnessStats => Set<UserFitnessStat>();
        public DbSet<CalculatedMetric> CalculatedMetrics => Set<CalculatedMetric>();
        public DbSet<FitnessPlanConfig> FitnessPlanConfigs => Set<FitnessPlanConfig>();
        public DbSet<UserAssignedPlan> UserAssignedPlans => Set<UserAssignedPlan>();
        public DbSet<UserPlanHistory> UserPlanHistories => Set<UserPlanHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<FitnessPlanConfig>().HasData(FitnessPlanConfigSeed.GetSeedData());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FCEDbContext).Assembly);

           
        }
    }
}
