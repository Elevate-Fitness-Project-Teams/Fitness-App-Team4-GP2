namespace ProfileService.Domain.Entities
{
    /// <summary>
    /// Local denormalized read model for the View Profile endpoint. Populated asynchronously
    /// from RabbitMQ events: the progress stats come from ProgressService and the fitness
    /// fields (height/weight) come from FCEService. Read directly at query time — no
    /// synchronous cross-service calls.
    /// </summary>
    public class UserStatisticsSnapshot
    {
        public Guid UserId { get; set; }

        // From ProgressService (UserStatisticsUpdatedEvent)
        public int TotalWorkouts { get; set; }
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public int TotalCaloriesBurned { get; set; }
        public double TotalWeightLost { get; set; }

        // From FCEService (UserFitnessUpdatedEvent)
        public double? Height { get; set; }
        public double? Weight { get; set; }

        // Per-source freshness timestamps (also used to drop stale/out-of-order events).
        public DateTime? StatsUpdatedAt { get; set; }
        public DateTime? FitnessUpdatedAt { get; set; }
    }
}
