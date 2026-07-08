using ProfileService.Domain.Enums;

namespace ProfileService.Features.ViewSettings.Dtos
{
    public class PrivacySettingsDto
    {
        public string ProfileVisibility { get; set; } = default(string)!;
        public bool ShowProgressToFriends { get; set; } = false;
        public bool AllowDataSharing { get; set; } = false;
    }
}
