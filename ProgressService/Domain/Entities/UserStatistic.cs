using System;
namespace ProgressService.Domain.Entities
{
    public class UserStatistic
    {
        public int UserId { get; set; }
        public int TotalWorkouts { get; set; } = 0;
        public int TotalCaloriesBurned { get; set; } = 0;
        public double TotalWeightLost { get; set; } = 0;
        public DateTime UpdatedAt { get; set; }
    }
}