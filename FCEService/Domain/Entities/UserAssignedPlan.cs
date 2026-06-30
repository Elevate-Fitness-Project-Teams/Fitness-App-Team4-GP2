using System;
namespace FCEService.Domain.Entities
{
    public class UserAssignedPlan
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string PlanId { get; set; } = null!;
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; }
      
        public FitnessPlanConfig FitnessPlanConfig { get; set; } = null!;
      
    }
}
