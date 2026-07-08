using FCEService.Domain.Enums;

namespace FCEService.Domain.Entities
{
    public class CalculatedMetric : BaseEntity
    {
       
        public Guid UserId { get; set; }
        public double Bmr { get; set; }
        public double Tdee { get; set; }
        public double CalorieTarget { get; set; }
        public PlanStatus Status { get; set; } 
        public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastUpdatedAt { get; set; }
    }
}
