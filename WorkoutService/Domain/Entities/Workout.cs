using WorkoutService.Domain.Enums;

namespace WorkoutService.Domain.Entities
{
    public class Workout
    {
        public int WorkoutId { get; set; }
        public string PlanId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public WorkOutCategory Category { get; set; }
        public int DurationInMinutes { get; set; }
        public WorkOutDiffeculty Difficulty { get; set; } 
        public WorkoutPlan WorkoutPlan { get; set; } = null!;
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();

    }
}
