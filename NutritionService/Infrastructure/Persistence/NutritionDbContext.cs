using Microsoft.EntityFrameworkCore;
using NutritionService.Domain.Entities;

namespace NutritionService.Infrastructure.Persistence
{
    public class NutritionDbContext : DbContext
    {
        public NutritionDbContext(DbContextOptions<NutritionDbContext> options) : base(options)
        {
        }

        public DbSet<Meal> Meals { get; set; } = null!;
        public DbSet<MealPlan> MealPlans { get; set; } = null!;
        public DbSet<MealPlanItem> MealPlanItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Meal>(entity =>
            {
                entity.HasKey(e => e.MealId);
                entity.Property(e => e.Name).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Type).HasMaxLength(20).IsRequired();
                entity.Property(e => e.IngredientsJson).IsRequired();
                entity.Property(e => e.InstructionsJson).IsRequired();
                entity.Property(e => e.VariationsJson).IsRequired(false);
                entity.Property(e => e.AllergensJson).HasMaxLength(500).IsRequired();
                entity.Property(e => e.TagsJson).HasMaxLength(255).IsRequired();
            });

            modelBuilder.Entity<MealPlan>(entity =>
            {
                entity.HasKey(e => e.MealPlanId);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
            });

            modelBuilder.Entity<MealPlanItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DayOfWeek).HasMaxLength(15).IsRequired();
                entity.Property(e => e.MealTime).HasMaxLength(20).IsRequired();

                entity.HasOne(e => e.MealPlan)
                    .WithMany()
                    .HasForeignKey(e => e.MealPlanId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Meal)
                    .WithMany()
                    .HasForeignKey(e => e.MealId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
