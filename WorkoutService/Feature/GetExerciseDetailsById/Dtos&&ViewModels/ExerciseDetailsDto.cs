namespace WorkoutService.Feature.GetExerciseDetailsById.Dtos__ViewModels
{
    public class ExerciseDetailsDto
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; } = null!;
        public string TargetMuscles { get; set; } = null!;
        public string Equipment { get; set; } = null!;
        public string Difficulty { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? VideoUrl { get; set; }
    }
}
