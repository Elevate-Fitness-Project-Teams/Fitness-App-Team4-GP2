using ProfileService.Features.ViewSettings.Dtos;

namespace ProfileService.Features.UpdateSettings.Dtos
{
    public class UpdateSettingsRequest
    {
        public PreferenceSettingsDto? Preferences { get; set; }
        public UpdateNotificationDto? Notifications { get; set; }
        public UpdatePrivacySettingsDto? Privacy { get; set; }
    }
}
