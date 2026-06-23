using Microsoft.EntityFrameworkCore;
using SmartCoachService.Domain.Entities;

namespace SmartCoachService.Infrastructure.Persistence
{
    public class SmartCoachDbContext : DbContext
    {
        public SmartCoachDbContext(DbContextOptions<SmartCoachDbContext> options) : base(options)
        {
        }

        public DbSet<ChatSession> ChatSessions { get; set; } = null!;
        public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public DbSet<RecommendationCache> RecommendationCaches { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChatSession>(entity =>
            {
                entity.HasKey(e => e.SessionId);
                entity.Property(e => e.SessionId).HasMaxLength(100);
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.Title).HasMaxLength(150).HasDefaultValue("New Conversation").IsRequired();
            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(e => e.MessageId);
                entity.Property(e => e.SessionId).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Sender).HasMaxLength(10).IsRequired();
                entity.Property(e => e.Content).IsRequired();

                entity.HasOne(e => e.ChatSession)
                    .WithMany()
                    .HasForeignKey(e => e.SessionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RecommendationCache>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserContextJson).IsRequired();
                entity.Property(e => e.HomeFeedDataJson).IsRequired();
            });
        }
    }
}
