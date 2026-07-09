using BuildingBlocks.Contracts.Auth;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProfileService.Domain.Entities;

namespace ProfileService.Infrastructure.Persistence.Consumers
{
    public class UserProfileCompletedConsumer
    : IConsumer<UserProfileCompletedEvent>
    {
        private readonly ProfileDbContext _db;

        public UserProfileCompletedConsumer(ProfileDbContext db)
        {
            _db = db;
        }

        public async Task Consume(ConsumeContext<UserProfileCompletedEvent> context)
        {
            var message = context.Message;

            // Idempotency: skip if this user's profile was already provisioned
            // (MassTransit may redeliver, and the endpoint could be retried).
            if (await _db.UserProfiles.AnyAsync(p => p.UserId == message.UserId))
                return;

            await _db.UserProfiles.AddAsync(new UserProfile
            {
                UserId = message.UserId,
                Email = message.Email,
                FirstName = message.FirstName,
                LastName = message.LastName,
                PhoneNumber = message.PhoneNumber,
                MemberSince = message.CompletedAt,
                IsPremiumCached = message.IsPremium
            });

            await _db.UserPreferences.AddAsync(new UserPreference
            {
                UserId = message.UserId,
                Language = "en",
                Theme = "light",
                WeightUnit = "kg",
                HeightUnit = "cm",
                DistanceUnit = "km"
            });

            await _db.PrivacySettings.AddAsync(new PrivacySetting
            {
                UserId = message.UserId,
                AllowDataSharing = false,
                ProfileVisibility = Domain.Enums.ProfileVisibilityEnum.Private,
                ShowProgressToFriends = false
            });

            await _db.NotificationSettings.AddAsync(new NotificationSetting
            {
                UserId = message.UserId,
                AchievementAlerts = true,
                PushNotifications = true,
                EmailNotifications = true,
                MealReminders = true, 
                WeeklyReports = true,
                WorkoutReminders = true,
            });

            await _db.SaveChangesAsync();
        }
    }
}
