using FCEService.Domain.Entities;
using FCEService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCEService.Infrastructure.Data.Configurations
{
    public sealed class FitnessPlanConfigConfiguration
        : IEntityTypeConfiguration<FitnessPlanConfig>
    {
        public void Configure(EntityTypeBuilder<FitnessPlanConfig> builder)
        {
            builder.ToTable("FitnessPlanConfigs");

            builder.HasKey(x => x.PlanId);

            builder.Property(x => x.PlanId)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.PlanName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Description)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(x => x.Goal)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(50);

            builder.Property(x => x.Status)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(x => x.MinCalorie)
                   .IsRequired()
                   .HasColumnType("float");

            builder.Property(x => x.MaxCalorie)
                   .IsRequired()
                   .HasColumnType("float");

            builder.Property(x => x.EstimatedDuration)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.WorkoutsPerWeek)
                   .IsRequired();

            builder.Property(x => x.ProgramType)
                   .HasMaxLength(50)
                   .IsRequired();

            // Navigation
            builder.HasMany(x => x.UserAssignedPlans)
                   .WithOne(x => x.FitnessPlanConfig)
                   .HasForeignKey(x => x.PlanId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
