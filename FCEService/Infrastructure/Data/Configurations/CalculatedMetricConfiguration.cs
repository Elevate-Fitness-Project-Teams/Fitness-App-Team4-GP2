using FCEService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCEService.Infrastructure.Data.Configurations
{
    public sealed class CalculatedMetricConfiguration
          : IEntityTypeConfiguration<CalculatedMetric>
    {
        public void Configure(EntityTypeBuilder<CalculatedMetric> builder)
        {
            builder.ToTable("CalculatedMetrics");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .UseIdentityColumn();

            // Unique: exactly one active metric row per user (upsert on recalculate)
            builder.HasIndex(x => x.UserId)
                   .IsUnique()
                   .HasDatabaseName("UIX_CalculatedMetrics_UserId");

            builder.Property(x => x.UserId)
                   .IsRequired();

            builder.Property(x => x.Bmr)
                   .IsRequired()
                   .HasColumnType("float");

            builder.Property(x => x.Tdee)
                   .IsRequired()
                   .HasColumnType("float");

            builder.Property(x => x.CalorieTarget)
                   .IsRequired()
                   .HasColumnType("float");

            // PlanStatus enum → string in DB
            builder.Property(x => x.Status)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(x => x.CalculatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            // Nullable — null means never recalculated after first insert
            builder.Property(x => x.LastUpdatedAt)
                   .IsRequired(false);
        }
    }
}