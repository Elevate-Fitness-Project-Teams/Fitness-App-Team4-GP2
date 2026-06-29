using FCEService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCEService.Infrastructure.Persistence.Configurations
{
    public sealed class UserFitnessStatConfiguration
        : IEntityTypeConfiguration<UserFitnessStat>
    {
        public void Configure(EntityTypeBuilder<UserFitnessStat> builder)
        {
            builder.ToTable("UserFitnessStats");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .UseIdentityColumn();

            // UserId is indexed (not unique) — one user can have many stat logs
            builder.HasIndex(x => x.UserId)
                   .HasDatabaseName("IX_UserFitnessStats_UserId");

            builder.Property(x => x.UserId)
                   .IsRequired();

            builder.Property(x => x.Weight)
                   .IsRequired()
                   .HasColumnType("float");

            builder.Property(x => x.Height)
                   .IsRequired()
                   .HasColumnType("float");

            builder.Property(x => x.Age)
                   .IsRequired();

            builder.Property(x => x.Gender)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(10);

            builder.Property(x => x.Goal)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(50);

            builder.Property(x => x.ActivityLevel)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(30);

            builder.Property(x => x.RecordedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}