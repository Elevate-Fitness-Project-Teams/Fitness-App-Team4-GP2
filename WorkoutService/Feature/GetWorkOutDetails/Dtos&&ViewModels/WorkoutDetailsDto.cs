using WorkoutService.Domain.Enums;

namespace WorkoutService.Feature.GetWorkOutDetails.Dtos__ViewModels
{
    public class WorkoutDetailsDto
    {
        public int WorkoutId { get; set; }

        public string PlanId { get; set; } = null!;

        public string WorkoutName { get; set; } = null!;

        public WorkOutCategory WorkoutCategory { get; set; }

        public int DurationInMinutes { get; set; }

        public WorkOutDiffeculty WorkoutDifficulty { get; set; }

        public List<WorkoutExerciseDto> Exercises { get; set; } = [];
    }
}
