using Microsoft.EntityFrameworkCore;
using FCEService.Domain.Entities;

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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FCEDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
