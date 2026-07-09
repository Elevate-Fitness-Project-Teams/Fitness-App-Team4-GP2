using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProfileService.BuildingBlocks.Interfaces;
using ProfileService.Domain.Entities;
using ProfileService.Features.UpdateSettings.Dtos;

namespace ProfileService.Features.UpdateSettings
{
    public class UpdateSettingsCommandHandler : IRequestHandler<UpdateSettingsCommand, Result<UpdateSettingsResponse>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IGenericRepository<UserPreference> _preferenceRepo;
        private readonly IGenericRepository<NotificationSetting> _notificationRepo;
        private readonly IGenericRepository<PrivacySetting> _privacyRepo;

        public UpdateSettingsCommandHandler(ICurrentUser currentUser,
            IGenericRepository<UserPreference> preferenceRepo,
            IGenericRepository<NotificationSetting> notificationRepo,
            IGenericRepository<PrivacySetting> privacyRepo)
        {
            _currentUser = currentUser;
            _preferenceRepo = preferenceRepo;
            _notificationRepo = notificationRepo;
            _privacyRepo = privacyRepo;
        }

        public async Task<Result<UpdateSettingsResponse>> Handle(UpdateSettingsCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(_currentUser.UserId, out var userId))
                return Error.Unauthorized("AUTH_TOKEN_INVALID", "Authentication token is missing or invalid.");

            var req = request.Request;

            // True PATCH: at least one section must be supplied.
            if (req.Preferences is null && req.Notifications is null && req.Privacy is null)
                return Error.Validation("VAL_REQUIRED_FIELD", "No settings fields were supplied.");

            // Only the sections actually being patched are loaded (tracked, for update) —
            // untouched tables are never queried.
            if (req.Preferences is not null)
            {
                var preferences = await _preferenceRepo
                    .GetAllAsync(x => x.UserId == userId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (preferences is null)
                    return Error.NotFound("RES_NOT_FOUND", "User preferences not found.");

                var dto = req.Preferences;

                if (dto.Language is not null) preferences.Language = dto.Language;
                if (dto.Theme is not null) preferences.Theme = dto.Theme;
                if (dto.WeightUnit is not null) preferences.WeightUnit = dto.WeightUnit;
                if (dto.HeightUnit is not null) preferences.HeightUnit = dto.HeightUnit;
                if (dto.DistanceUnit is not null) preferences.DistanceUnit = dto.DistanceUnit;

                _preferenceRepo.Update(preferences);
            }

            if (req.Notifications is not null)
            {
                var notifications = await _notificationRepo
                    .GetAllAsync(x => x.UserId == userId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (notifications is null)
                    return Error.NotFound("RES_NOT_FOUND", "Notification settings not found.");

                var dto = req.Notifications;

                if (dto.WorkoutReminders.HasValue) notifications.WorkoutReminders = dto.WorkoutReminders.Value;
                if (dto.MealReminders.HasValue) notifications.MealReminders = dto.MealReminders.Value;
                if (dto.AchievementAlerts.HasValue) notifications.AchievementAlerts = dto.AchievementAlerts.Value;
                if (dto.WeeklyReports.HasValue) notifications.WeeklyReports = dto.WeeklyReports.Value;
                if (dto.EmailNotifications.HasValue) notifications.EmailNotifications = dto.EmailNotifications.Value;
                if (dto.PushNotifications.HasValue) notifications.PushNotifications = dto.PushNotifications.Value;

                _notificationRepo.Update(notifications);
            }

            if (req.Privacy is not null)
            {
                var privacy = await _privacyRepo
                    .GetAllAsync(x => x.UserId == userId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (privacy is null)
                    return Error.NotFound("RES_NOT_FOUND", "Privacy settings not found.");

                var dto = req.Privacy;

                if (dto.ProfileVisibility.HasValue) privacy.ProfileVisibility = dto.ProfileVisibility.Value;
                if (dto.ShowProgressToFriends.HasValue) privacy.ShowProgressToFriends = dto.ShowProgressToFriends.Value;
                if (dto.AllowDataSharing.HasValue) privacy.AllowDataSharing = dto.AllowDataSharing.Value;

                _privacyRepo.Update(privacy);
            }

            await _preferenceRepo.SaveChangesAsync();

            return new UpdateSettingsResponse(true);
        }
    }
}
