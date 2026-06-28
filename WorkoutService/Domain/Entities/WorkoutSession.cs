using System;
namespace WorkoutService.Domain.Entities
{
    public class WorkoutSession
    {
        public string SessionId { get; set; } = null!;
        public Guid UserId { get; set; }
        public int WorkoutId { get; set; }
        public DateTime StartedAt { get; set; }
        public string Status { get; set; } = null!;
        // Navigation properties
        public Workout Workout { get; set; } = null!;
    }
}
