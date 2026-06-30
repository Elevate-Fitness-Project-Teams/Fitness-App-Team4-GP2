using WorkoutService.Domain.Enums;

namespace WorkoutService.Feature.GetWorkOutDetails.Dtos__ViewModels
{
    public class WorkoutDetailsViewModel
    {
        public int WorkoutId { get; set; }

        public string PlanId { get; set; } = null!;

        public string WorkoutName { get; set; } = null!;

        public string WorkoutCategory { get; set; } = null!;

        public int DurationInMinutes { get; set; }

        public string WorkoutDifficulty { get; set; } = null!;
        public List<WorkoutExerciseViewModle> Exercises { get; set; } = [];
    }
}
