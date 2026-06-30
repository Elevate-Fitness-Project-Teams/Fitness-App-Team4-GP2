namespace WorkoutService.Feature.StartWorkOutSession.Dtos__ViewModels
{
    public class WorkoutExerciseInSessionDto
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public string Name { get; set; } = null!;
        public string TargetMuscles { get; set; } = null!;
        public string Equipment { get; set; } = null!;
        public string Difficulty { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? VideoUrl { get; set; }
        public int SetsDefault { get; set; }
        public int RepsDefault { get; set; }
        public int RestTimeInSeconds { get; set; }
        public int OrderIndex { get; set; }
    }
}
