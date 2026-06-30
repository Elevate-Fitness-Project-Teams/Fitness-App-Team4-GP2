namespace WorkoutService.Feature.GetWorkOutDetails.Dtos__ViewModels
{
    public class WorkoutExerciseViewModle
    {
        public int ExerciseId { get; set; }

        public string ExerciseName { get; set; } = null!;

        public string TargetMuscles { get; set; } = null!;

        public string ExerciseEquipment { get; set; } = null!;

        public string ExerciseDifficulty { get; set; } = null!;

        public string ExerciseDescription { get; set; } = null!;

        public string? ExerciseVideoUrl { get; set; }

        public int SetsDefault { get; set; }

        public int RepsDefault { get; set; }

        public int RestTimeInSeconds { get; set; }
        public int OrderIndex { get; set; }
    }
}
