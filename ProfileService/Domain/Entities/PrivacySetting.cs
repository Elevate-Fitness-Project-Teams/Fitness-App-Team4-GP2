namespace ProfileService.Domain.Entities
{
    public class PrivacySetting
    {
        public Guid UserId { get; set; }
        public string ProfileVisibility { get; set; } = "private";
        public bool ShowProgressToFriends { get; set; } = false;
        public bool AllowDataSharing { get; set; } = false;
        // Navigation property to UserProfile
        public UserProfile UserProfile { get; set; } = null!;
    }
}
