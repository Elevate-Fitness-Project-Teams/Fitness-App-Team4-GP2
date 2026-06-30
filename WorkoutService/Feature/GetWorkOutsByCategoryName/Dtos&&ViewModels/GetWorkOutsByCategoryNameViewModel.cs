using WorkoutService.Domain.Enums;

namespace WorkoutService.Feature.GetWorkOutsByCategoryName.Dtos__ViewModels
{
    public class GetWorkOutsByCategoryNameViewModel
    {
        public int WorkoutId { get; set; }
        public string PlanId { get; set; } = null!;
        public string WorkoutName { get; set; } = null!;
        public string WorkoutCategory { get; set; } = null!;
        public string WorkoutDifficulty { get; set; } = null!;
        public int DurationInMinutes { get; set; }

        public string PlanName { get; set; } = null!;
        public string PlanDescription { get; set; } = null!;
        public string PlanGoal { get; set; } = null!;
        public string PlanStatus { get; set; } = null!;
    }
}
