using FCEService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCEService.Infrastructure.Data.Configurations
{
    public sealed class UserAssignedPlanConfiguration
         : IEntityTypeConfiguration<UserAssignedPlan>
    {
        public void Configure(EntityTypeBuilder<UserAssignedPlan> builder)
        {
            builder.ToTable("UserAssignedPlans");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .UseIdentityColumn();

            builder.HasIndex(x => x.UserId)
                   .HasDatabaseName("IX_UserAssignedPlans_UserId");

            // Composite index: fast lookup of the active plan for a user
            builder.HasIndex(x => new { x.UserId, x.IsActive })
                   .HasDatabaseName("IX_UserAssignedPlans_UserId_IsActive");

            builder.Property(x => x.UserId)
                   .IsRequired();

            builder.Property(x => x.PlanId)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.AssignedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

            // FK → FitnessPlanConfig (restrict: never delete a plan that has assignments)
            builder.HasOne(x => x.FitnessPlanConfig)
                   .WithMany(x => x.UserAssignedPlans)
                   .HasForeignKey(x => x.PlanId)
                   .OnDelete(DeleteBehavior.Restrict);

          
        }
    }
}
