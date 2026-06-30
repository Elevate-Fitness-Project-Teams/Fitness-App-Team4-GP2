using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;

namespace WorkoutService.Feature.GetWorkOutsByCategoryName.Dtos__ViewModels
{
    public class WorkoutByCategoryDto
    {
        public int WorkoutId { get; set; }
        public string PlanId { get; set; } = null!;
        public string WorkoutName { get; set; } = null!;
        public WorkOutCategory WorkoutCategory { get; set; }
        public WorkOutDiffeculty WorkoutDifficulty { get; set; }
        public int DurationInMinutes { get; set; }

        public string PlanName { get; set; } = null!;
        public string PlanDescription { get; set; } = null!;
        public string PlanGoal { get; set; } = null!;
        public string PlanStatus { get; set; } = null!;
    }
}
