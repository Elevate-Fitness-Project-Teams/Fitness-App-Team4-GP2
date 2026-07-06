using Microsoft.EntityFrameworkCore;
using ProfileService.Domain.Entities;
using ProfileService.Domain.Enums;

namespace ProfileService.Infrastructure.Persistence
{
    public class ProfileDbContext : DbContext
    {
        public ProfileDbContext(DbContextOptions<ProfileDbContext> options) : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; } = null!;
        public DbSet<UserPreference> UserPreferences { get; set; } = null!;
        public DbSet<NotificationSetting> NotificationSettings { get; set; } = null!;
        public DbSet<PrivacySetting> PrivacySettings { get; set; } = null!;
        public DbSet<UserStatisticsSnapshot> UserStatisticsSnapshots { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
                entity.Property(e => e.PhoneNumber).HasMaxLength(20).IsRequired();
                entity.Property(e => e.ProfilePictureUrl).HasMaxLength(500).IsRequired(false);
                entity.Property(e => e.IsPremiumCached).IsRequired();
                entity.Property(e => e.MemberSince).IsRequired();
            });

            modelBuilder.Entity<UserPreference>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Language).HasMaxLength(10).HasDefaultValue("en").IsRequired();
                entity.Property(e => e.Theme).HasMaxLength(15).HasDefaultValue("light").IsRequired();
                entity.Property(e => e.WeightUnit).HasMaxLength(5).HasDefaultValue("kg").IsRequired();
                entity.Property(e => e.HeightUnit).HasMaxLength(5).HasDefaultValue("cm").IsRequired();
                entity.Property(e => e.DistanceUnit).HasMaxLength(5).HasDefaultValue("km").IsRequired();

                entity.HasOne(e => e.UserProfile)
                    .WithOne()
                    .HasForeignKey<UserPreference>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<NotificationSetting>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.WorkoutReminders).HasDefaultValue(true).IsRequired();
                entity.Property(e => e.MealReminders).HasDefaultValue(true).IsRequired();
                entity.Property(e => e.AchievementAlerts).HasDefaultValue(true).IsRequired();
                entity.Property(e => e.WeeklyReports).HasDefaultValue(true).IsRequired();
                entity.Property(e => e.EmailNotifications).HasDefaultValue(true).IsRequired();
                entity.Property(e => e.PushNotifications).HasDefaultValue(true).IsRequired();

                entity.HasOne(e => e.UserProfile)
                    .WithOne()
                    .HasForeignKey<NotificationSetting>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserStatisticsSnapshot>(entity =>
            {
                entity.HasKey(e => e.UserId);
            });

            modelBuilder.Entity<PrivacySetting>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.ProfileVisibility).HasConversion<string>()
                            .HasDefaultValue(ProfileVisibilityEnum.Private)
                            .HasMaxLength(20)
                            .IsRequired();

                entity.Property(e => e.ShowProgressToFriends).HasDefaultValue(false).IsRequired();
                entity.Property(e => e.AllowDataSharing).HasDefaultValue(false).IsRequired();

                entity.HasOne(e => e.UserProfile)
                    .WithOne()
                    .HasForeignKey<PrivacySetting>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
