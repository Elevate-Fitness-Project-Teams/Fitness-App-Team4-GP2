using System;
using WorkoutService.Domain.Enums;
namespace WorkoutService.Domain.Entities
{
    public class WorkoutSession
    {
        public string SessionId { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } = null!;
        public int WorkoutId { get; set; }
        public DateTime StartedAt { get; set; }
        public WorkOutSessionStatus Status { get; set; }
        // Navigation properties
        public Workout Workout { get; set; } = null!;
    }
}
