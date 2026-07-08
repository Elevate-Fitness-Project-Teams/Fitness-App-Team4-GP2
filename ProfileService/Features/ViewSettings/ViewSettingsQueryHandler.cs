using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProfileService.BuildingBlocks.Interfaces;
using ProfileService.Domain.Entities;
using ProfileService.Features.ViewSettings.Dtos;

namespace ProfileService.Features.ViewSettings
{
    public class ViewSettingsQueryHandler : IRequestHandler<ViewSettingsQuery, Result<ViewSettingsResponse>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IGenericRepository<UserPreference> _preferencesRepo;
        private readonly IGenericRepository<NotificationSetting> _notificationRepo;
        private readonly IGenericRepository<PrivacySetting> _privacyRepo;

        public ViewSettingsQueryHandler(
            ICurrentUser currentUser,
            IGenericRepository<UserPreference> preferencesRepo,
            IGenericRepository<NotificationSetting> notificationRepo,
            IGenericRepository<PrivacySetting> privacyRepo)
        {
            _currentUser = currentUser;
            _preferencesRepo = preferencesRepo;
            _notificationRepo = notificationRepo;
            _privacyRepo = privacyRepo;
        }

        public async Task<Result<ViewSettingsResponse>> Handle(
            ViewSettingsQuery request,
            CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(_currentUser.UserId, out var userId))
                return Error.Unauthorized("AUTH_TOKEN_INVALID", "Authentication token is missing or invalid.");

            var preferences = await _preferencesRepo
                .GetAllAsync(x => x.UserId == userId)
                .Select(x => new
                {
                    x.DistanceUnit,
                    x.HeightUnit,
                    x.WeightUnit,
                    x.Language,
                    x.Theme
                })
                .FirstOrDefaultAsync(cancellationToken);

            var notifications = await _notificationRepo
                .GetAllAsync(x => x.UserId == userId)
                .Select(x => new
                {
                    x.PushNotifications,
                    x.WorkoutReminders,
                    x.MealReminders,
                    x.AchievementAlerts,
                    x.WeeklyReports,
                    x.EmailNotifications,
                })
                .FirstOrDefaultAsync(cancellationToken);

            var privacy = await _privacyRepo
                .GetAllAsync(x => x.UserId == userId)
                .Select(x => new
                {
                    x.ProfileVisibility,
                    x.ShowProgressToFriends,
                    x.AllowDataSharing
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (preferences is null || notifications is null || privacy is null)
                return Error.NotFound("RES_NOT_FOUND", "User settings not found.");

            return new ViewSettingsResponse
            {
                UserPreferences = new UserPreferencesDto
                {
                    WeightUnit = preferences.WeightUnit, 
                    Language = preferences.Language,
                    Theme = preferences.Theme,
                    DistanceUnit = preferences.DistanceUnit,
                    HeightUnit = preferences.HeightUnit
                },

                NotificationSettings = new NotificationSettingsDto
                {
                    WorkoutReminders = notifications.WorkoutReminders,
                    MealReminders = notifications.MealReminders,
                    AchievementAlerts = notifications.AchievementAlerts,
                    WeeklyReports = notifications.WeeklyReports,
                    EmailNotifications = notifications.EmailNotifications,
                    PushNotifications = notifications.PushNotifications
                },

                PrivacySettings = new PrivacySettingsDto
                {
                    ProfileVisibility = privacy.ProfileVisibility.ToString(),
                    ShowProgressToFriends = privacy.ShowProgressToFriends,
                    AllowDataSharing = privacy.AllowDataSharing
                }
            };
        }
    }
}
