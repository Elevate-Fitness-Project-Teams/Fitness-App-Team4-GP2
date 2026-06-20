using System;
using Elevate.FitnessApp.Features.Auth.Entities;
namespace Elevate.FitnessApp.Features.Progress.Entities
{
    public class UserStatistic
    {
        public int UserId { get; set; }
        public int TotalWorkouts { get; set; } = 0;
        public int TotalCaloriesBurned { get; set; } = 0;
        public double TotalWeightLost { get; set; } = 0;
        public DateTime UpdatedAt { get; set; }
        // Navigation property
        public User User { get; set; } = null!;
    }
}