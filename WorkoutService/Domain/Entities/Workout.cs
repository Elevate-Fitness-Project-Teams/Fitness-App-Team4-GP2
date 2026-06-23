namespace WorkoutService.Domain.Entities
{
    public class Workout
    {
        public int WorkoutId { get; set; }
        public string PlanId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public int DurationInMinutes { get; set; }
        public string Difficulty { get; set; } = null!;
        public WorkoutPlan WorkoutPlan { get; set; } = null!;
    }
}
