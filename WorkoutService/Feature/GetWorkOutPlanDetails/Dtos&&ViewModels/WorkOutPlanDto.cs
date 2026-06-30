using WorkoutService.Domain.Enums;

namespace WorkoutService.Feature.GetWorkOutPlanDetails.Dtos__ViewModels
{
    public class WorkOutPlanDto
    {
        public string PlanId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Goal { get; set; } = null!;
        public string Status { get; set; } = null!;
        public WorkOutPlanDifficulty Difficulty { get; set; }
    }
}
