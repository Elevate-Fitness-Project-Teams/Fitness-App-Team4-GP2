namespace ProgressService.Features.ViewUserAchievements
{
    public class UserAchievementsResponse
    {
        public List<AchievementEntry> Achievements { get; set; } = new List<AchievementEntry>();
        public class AchievementEntry
        {
            public string Name { get; set; } = null!;
            public string Description { get; set; } = null!;
            public string IconUrl { get; set; }  = null!;
            public DateTime EarnedAt { get; set; }
        }
    }
}