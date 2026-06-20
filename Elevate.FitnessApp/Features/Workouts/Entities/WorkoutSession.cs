using System;
using Elevate.FitnessApp.Features.Auth.Entities;
namespace Elevate.FitnessApp.Features.Workouts.Entities
{
    public class WorkoutSession
    {
        public string SessionId { get; set; } = null!;
        public int UserId { get; set; }
        public int WorkoutId { get; set; }
        public DateTime StartedAt { get; set; }
        public string Status { get; set; } = null!;
        // Navigation properties
        public User User { get; set; } = null!;
        public Workout Workout { get; set; } = null!;
    }
}
