namespace Elevate.FitnessApp.Features.Progress.Entities
{
    public class WorkoutLogExercise
    {
        public int Id { get; set; }
        public int WorkoutLogId { get; set; }
        public int ExerciseId { get; set; }
        public int SetsCompleted { get; set; }
        public int RepsCompleted { get; set; }
        public double WeightUsed { get; set; } = 0;
        public bool Completed { get; set; }
        // Navigation property
        public WorkoutLog WorkoutLog { get; set; } = null!;
    }
}
