using Microsoft.EntityFrameworkCore;
using FCEService.Domain.Entities;

namespace FCEService.Infrastructure.Persistence
{
    public class FCEDbContext : DbContext
    {
        public FCEDbContext(DbContextOptions<FCEDbContext> options) : base(options)
        {
        }

        public DbSet<UserFitnessStat> UserFitnessStats { get; set; } = null!;
        public DbSet<CalculatedMetric> CalculatedMetrics { get; set; } = null!;
        public DbSet<FitnessPlanConfig> FitnessPlanConfigs { get; set; } = null!;
        public DbSet<UserAssignedPlan> UserAssignedPlans { get; set; } = null!;
        public DbSet<UserPlanHistory> UserPlanHistories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserFitnessStat>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.Gender).HasMaxLength(10).IsRequired();
                entity.Property(e => e.Goal).HasMaxLength(50).IsRequired();
                entity.Property(e => e.ActivityLevel).HasMaxLength(30).IsRequired();
                entity.Property(e => e.RecordedAt).IsRequired();
            });

            modelBuilder.Entity<CalculatedMetric>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId).IsUnique();
                entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
                entity.Property(e => e.CalculatedAt).IsRequired();
            });

            modelBuilder.Entity<FitnessPlanConfig>(entity =>
            {
                entity.HasKey(e => e.PlanId);
                entity.Property(e => e.PlanId).HasMaxLength(50);
                entity.Property(e => e.PlanName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
                entity.Property(e => e.Goal).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
                entity.Property(e => e.EstimatedDuration).HasMaxLength(100).IsRequired();
                entity.Property(e => e.WorkoutsPerWeek).IsRequired();
                entity.Property(e => e.ProgramType).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<UserAssignedPlan>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.PlanId).HasMaxLength(50).IsRequired();
                entity.Property(e => e.AssignedAt).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();

                entity.HasOne(e => e.FitnessPlanConfig)
                    .WithMany()
                    .HasForeignKey(e => e.PlanId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UserPlanHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.PlanId).HasMaxLength(50).IsRequired();
                entity.Property(e => e.AssignedAt).IsRequired();
                entity.Property(e => e.EndedAt).IsRequired(false);
                entity.Property(e => e.ReasonForChange).HasMaxLength(255).IsRequired();
            });
        }
    }
}
