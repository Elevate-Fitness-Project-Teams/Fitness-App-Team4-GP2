namespace ProgressService.Shared.Events
{
    public class AchievementEarnedEvent
    {
        public string UserId { get; init; } = default!;
        public int AchievementId { get; init; }
        public string AchievementName { get; init; } = default!;
        public DateTime EarnedAt { get; init; }
    }
}
