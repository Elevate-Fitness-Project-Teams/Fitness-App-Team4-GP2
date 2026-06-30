using WorkoutService.Domain.Enums;

namespace WorkoutService.Domain.Entities
{
    public class WorkoutPlan
    {
        public string PlanId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Goal { get; set; } = null!;
        public string Status { get; set; } = null!;
        public WorkOutPlanDifficulty Difficulty { get; set; } 
    }
}