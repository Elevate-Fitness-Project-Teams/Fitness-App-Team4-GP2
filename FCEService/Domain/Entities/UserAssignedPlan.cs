using System;
namespace FCEService.Domain.Entities
{
    public class UserAssignedPlan : BaseEntity

    {
     
        public Guid UserId { get; set; }
        public string PlanId { get; set; } = null!;
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; }
      
        public FitnessPlanConfig FitnessPlanConfig { get; set; } = null!;
      
    }
}
