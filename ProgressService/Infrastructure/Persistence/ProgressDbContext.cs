using Microsoft.EntityFrameworkCore;
using ProgressService.Domain.Entities;

namespace ProgressService.Infrastructure.Persistence
{
    public class ProgressDbContext : DbContext
    {
        public ProgressDbContext(DbContextOptions<ProgressDbContext> options) : base(options)
        {
        }

        public DbSet<WorkoutLog> WorkoutLogs { get; set; } = null!;
        public DbSet<WorkoutLogExercise> WorkoutLogExercises { get; set; } = null!;
        public DbSet<WeightHistory> WeightHistories { get; set; } = null!;
        public DbSet<BodyMeasurement> BodyMeasurements { get; set; } = null!;
        public DbSet<Achievement> Achievements { get; set; } = null!;
        public DbSet<UserAchievement> UserAchievements { get; set; } = null!;
        public DbSet<Streak> Streaks { get; set; } = null!;
        public DbSet<UserStatistic> UserStatistics { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WorkoutLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.SessionId);
                entity.Property(e => e.SessionId).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Notes).HasMaxLength(1000).IsRequired(false);
            });

            modelBuilder.Entity<WorkoutLogExercise>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.WeightUsed).HasDefaultValue(0);

                entity.HasOne(e => e.WorkoutLog)
                    .WithMany(w => w.WorkoutLogExercises)
                    .HasForeignKey(e => e.WorkoutLogId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WeightHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.Notes).HasMaxLength(500).IsRequired(false);
            });

            modelBuilder.Entity<BodyMeasurement>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<Achievement>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(255).IsRequired();
                entity.Property(e => e.IconUrl).HasMaxLength(255).IsRequired();
            });

            modelBuilder.Entity<UserAchievement>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);

                entity.HasOne(e => e.Achievement)
                    .WithMany()
                    .HasForeignKey(e => e.AchievementId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Streak>(entity =>
            {
                entity.Property(e => e.CurrentStreak).HasDefaultValue(0).IsRequired();
                entity.Property(e => e.LongestStreak).HasDefaultValue(0).IsRequired();
            });

            modelBuilder.Entity<UserStatistic>(entity =>
            {
                entity.Property(e => e.TotalWorkouts).HasDefaultValue(0).IsRequired();
                entity.Property(e => e.TotalCaloriesBurned).HasDefaultValue(0).IsRequired();
                entity.Property(e => e.TotalWeightLost).HasDefaultValue(0).IsRequired();
            });
        }
    }
}
