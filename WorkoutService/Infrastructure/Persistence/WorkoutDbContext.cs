using Microsoft.EntityFrameworkCore;
using WorkoutService.Domain.Entities;

namespace WorkoutService.Infrastructure.Persistence
{
    public class WorkoutDbContext : DbContext
    {
        public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options) : base(options)
        {
        }

        public DbSet<WorkoutPlan> WorkoutPlans { get; set; } = null!;
        public DbSet<Workout> Workouts { get; set; } = null!;
        public DbSet<Exercise> Exercises { get; set; } = null!;
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; } = null!;
        public DbSet<WorkoutSession> WorkoutSessions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WorkoutPlan>(entity =>
            {
                entity.HasKey(e => e.PlanId);
                entity.Property(e => e.PlanId).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
                entity.Property(e => e.Goal).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Difficulty).HasMaxLength(20).IsRequired();
            });

            modelBuilder.Entity<Workout>(entity =>
            {
                entity.HasKey(e => e.WorkoutId);
                entity.HasIndex(e => e.Category);
                entity.Property(e => e.PlanId).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Category).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Difficulty).HasMaxLength(20).IsRequired();

                entity.HasOne(e => e.WorkoutPlan)
                    .WithMany()
                    .HasForeignKey(e => e.PlanId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.HasKey(e => e.ExerciseId);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.TargetMuscles).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Equipment).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Difficulty).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(1000).IsRequired();
                entity.Property(e => e.VideoUrl).HasMaxLength(500).IsRequired(false);
            });

            modelBuilder.Entity<WorkoutExercise>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Workout)
                    .WithMany()
                    .HasForeignKey(e => e.WorkoutId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Exercise)
                    .WithMany()
                    .HasForeignKey(e => e.ExerciseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<WorkoutSession>(entity =>
            {
                entity.HasKey(e => e.SessionId);
                entity.Property(e => e.SessionId).HasMaxLength(100);
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.Status).HasMaxLength(20).IsRequired();

                entity.HasOne(e => e.Workout)
                    .WithMany()
                    .HasForeignKey(e => e.WorkoutId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
