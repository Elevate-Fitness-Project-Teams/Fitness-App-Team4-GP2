using FCEService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCEService.Infrastructure.Data.Configurations
{
    public sealed class UserPlanHistoryConfiguration
         : IEntityTypeConfiguration<UserPlanHistory>
    {
        public void Configure(EntityTypeBuilder<UserPlanHistory> builder)
        {
            builder.ToTable("UserPlanHistories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .UseIdentityColumn();

            builder.HasIndex(x => x.UserId)
                   .HasDatabaseName("IX_UserPlanHistories_UserId");

            builder.Property(x => x.UserId)
                   .IsRequired();

            builder.Property(x => x.PlanId)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.AssignedAt)
                   .IsRequired();

            // Nullable — null means this history entry is still active (not yet ended)
            builder.Property(x => x.EndedAt)
                   .IsRequired(false);

            // ReasonForChange enum → string in DB (VarChar 255 per docs)
            builder.Property(x => x.ReasonForChange)
                   .IsRequired()
                   .HasMaxLength(255);
        }
    }
}