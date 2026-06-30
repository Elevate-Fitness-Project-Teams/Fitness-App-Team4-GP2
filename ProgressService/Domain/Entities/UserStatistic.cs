using System;
namespace ProgressService.Domain.Entities
{
    public class UserStatistic : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public int TotalWorkouts { get; set; } = 0;
        public int TotalCaloriesBurned { get; set; } = 0;
        public double TotalWeightLost { get; set; } = 0;
        public DateTime UpdatedAt { get; set; }
    }
}