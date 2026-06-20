namespace Elevate.FitnessApp.Features.Workouts.Entities
{
    public class Workout
    {
        public int WorkoutId { get; set; }
        public string PlanId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public int DurationInMinutes { get; set; }
        public string Difficulty { get; set; } = null!;
        // Navigation property
        public WorkoutPlan WorkoutPlan { get; set; } = null!;
    }
}
