using ProfileService.Domain.Enums;

namespace ProfileService.Features.UpdateSettings.Dtos
{
    public class UpdatePrivacySettingsDto
    {
        public ProfileVisibilityEnum? ProfileVisibility { get; set; }
        public bool? ShowProgressToFriends { get; set; }
        public bool? AllowDataSharing { get; set; }
    }
}