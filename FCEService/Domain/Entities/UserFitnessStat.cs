using FCEService.Domain.Enums;
using System;
namespace FCEService.Domain.Entities
{
    public class UserFitnessStat
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; } = null!;
        public GoalEnum Goal { get; set; } 
        public ActivityLevelEnum ActivityLevel { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}
