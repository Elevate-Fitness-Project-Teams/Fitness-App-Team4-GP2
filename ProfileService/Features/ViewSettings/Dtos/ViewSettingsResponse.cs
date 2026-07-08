namespace ProfileService.Features.ViewSettings.Dtos
{
    public class ViewSettingsResponse
    {
        public UserPreferencesDto UserPreferences { get; init; } = default!;
        public NotificationSettingsDto NotificationSettings { get; init; } = default!;
        public PrivacySettingsDto PrivacySettings { get; init; } = default!;
    }
}
