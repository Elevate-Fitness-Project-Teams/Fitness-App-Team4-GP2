using Microsoft.EntityFrameworkCore;
using SubscriptionService.Domain.Entities;

namespace SubscriptionService.Infrastructure.Persistence
{
    public class SubscriptionDbContext : DbContext
    {
        public SubscriptionDbContext(DbContextOptions<SubscriptionDbContext> options) : base(options)
        {
        }

        public DbSet<UserSubscription> UserSubscriptions { get; set; } = null!;
        public DbSet<BillingLog> BillingLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserSubscription>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId).IsUnique();
                entity.Property(e => e.Tier).IsRequired();
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Active").IsRequired();
            });

            modelBuilder.Entity<BillingLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.ReferenceId).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Currency).HasMaxLength(3).HasDefaultValue("EGP").IsRequired();
                entity.Property(e => e.PaymentStatus).HasMaxLength(20).IsRequired();
            });
        }
    }
}
