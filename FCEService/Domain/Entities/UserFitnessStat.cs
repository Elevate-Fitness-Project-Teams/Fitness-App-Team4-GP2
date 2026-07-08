using FCEService.Domain.Enums;
using System;
namespace FCEService.Domain.Entities
{
    public class UserFitnessStat : BaseEntity
    {
      
        public Guid UserId { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public FitnessGoal Goal { get; set; } 
        public ActivityLevel ActivityLevel { get; set; }
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    }
}
