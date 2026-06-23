using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Persistence
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
        {
        }

        public DbSet<InAppNotification> InAppNotifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InAppNotification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.Title).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Message).HasMaxLength(500).IsRequired();
                entity.Property(e => e.Type).HasMaxLength(30).IsRequired();
                entity.Property(e => e.IsRead).HasDefaultValue(false).IsRequired();
            });
        }
    }
}
