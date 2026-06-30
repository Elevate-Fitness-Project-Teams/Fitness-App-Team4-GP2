namespace ProgressService.Features.WorkoutCompletion
{
    public class ExerciseCompletedDto
    {
        public int ExerciseId { get; set; }
        public int SetsCompleted { get; set; }
        public int RepsCompleted { get; set; }
        public double WeightUsed { get; set; }
    }

}