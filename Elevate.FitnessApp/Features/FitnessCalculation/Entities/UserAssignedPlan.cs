using System;
using Elevate.FitnessApp.Features.Auth.Entities;
namespace Elevate.FitnessApp.Features.FitnessCalculation.Entities
{
    public class UserAssignedPlan
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PlanId { get; set; } = null!;
        public DateTime AssignedAt { get; set; }
        public bool IsActive { get; set; }
        // Navigation properties
        public User User { get; set; } = null!;
        public FitnessPlanConfig FitnessPlanConfig { get; set; } = null!;
    }
}
