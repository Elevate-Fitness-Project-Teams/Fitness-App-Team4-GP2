using System;
using Elevate.FitnessApp.Features.Auth.Entities;
namespace Elevate.FitnessApp.Features.Progress.Entities
{
    public class Streak
    {
        public int UserId { get; set; }
        public int CurrentStreak { get; set; } = 0;
        public int LongestStreak { get; set; } = 0;
        public DateTime? LastWorkoutDate { get; set; }
        // Navigation property
        public User User { get; set; } = null!;
    }
}
