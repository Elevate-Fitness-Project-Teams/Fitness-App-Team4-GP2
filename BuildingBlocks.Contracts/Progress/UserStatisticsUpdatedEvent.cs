namespace BuildingBlocks.Contracts.Progress
{
    /// <summary>
    /// Published by ProgressService whenever a user's aggregated statistics change.
    /// Consumed by ProfileService to refresh its cached statistics snapshot.
    /// </summary>
    public class UserStatisticsUpdatedEvent
    {
        public Guid UserId { get; init; }
        public int TotalWorkouts { get; init; }
        public int CurrentStreak { get; init; }
        public int LongestStreak { get; init; }
        public int TotalCaloriesBurned { get; init; }
        public double TotalWeightLost { get; init; }
        public DateTime UpdatedAt { get; init; }
    }
}
