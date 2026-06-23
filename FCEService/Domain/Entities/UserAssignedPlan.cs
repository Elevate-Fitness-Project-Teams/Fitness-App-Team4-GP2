using System;
namespace FCEService.Domain.Entities
{
    public class UserAssignedPlan
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PlanId { get; set; } = null!;
        public DateTime AssignedAt { get; set; }
        public bool IsActive { get; set; }
        // Navigation properties
        public FitnessPlanConfig FitnessPlanConfig { get; set; } = null!;
    }
}
