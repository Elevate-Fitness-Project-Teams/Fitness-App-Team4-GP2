using FCEService.Domain.Enums;

namespace FCEService.Domain.Entities
{
    public class FitnessPlanConfig
    {
        public string PlanId { get; set; } = null!;
        public string PlanName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public FitnessGoal Goal { get; set; } 
        public PlanStatus Status { get; set; } 
        public double MinCalorie { get; set; }
        public double MaxCalorie { get; set; }
        public string EstimatedDuration { get; set; } = null!;
        public int WorkoutsPerWeek { get; set; }
        public string ProgramType { get; set; } = null!;
        public ICollection<UserAssignedPlan> UserAssignedPlans { get; set; } = [];
    }
}
