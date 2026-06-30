using WorkoutService.Domain.Enums;

namespace WorkoutService.Feature.StartWorkOutSession.Dtos__ViewModels
{
    public class StartWorkOutSessionResultViewModel
    {
        public string SessionId { get; set; } = null!;
        public string Status { get; set; }

        public ICollection<WorkoutExerciseInSessionViewModel> ExercisesInSession { get; set; } = new List<WorkoutExerciseInSessionViewModel>();
    }
    public class WorkoutExerciseInSessionViewModel
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
