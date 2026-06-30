namespace ProgressService.Features.WorkoutCompletion
{
    public class WorkOutCompletionResponse
    {
        public int LogId { get; set; }
        public bool UpdatedStreak { get; set; }
        public int CurrentStreak { get; set; }

    }

}