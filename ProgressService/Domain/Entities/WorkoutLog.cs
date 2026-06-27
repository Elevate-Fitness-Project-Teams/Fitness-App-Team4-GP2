using System;
namespace ProgressService.Domain.Entities
{
    public class WorkoutLog : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public int WorkoutId { get; set; }
        public string SessionId { get; set; } = null!;
        public int DurationInMinutes { get; set; }
        public int CaloriesBurned { get; set; }
        public int Rating { get; set; }
        public string? Notes { get; set; }
        public DateTime CompletedAt { get; set; }

        public ICollection<WorkoutLogExercise> WorkoutLogExercises { get; set; } = [];

    }
}
