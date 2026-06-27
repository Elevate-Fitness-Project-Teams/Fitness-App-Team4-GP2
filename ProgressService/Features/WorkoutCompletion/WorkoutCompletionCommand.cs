using MediatR;
using ProgressService.Shared;

namespace ProgressService.Features.WorkoutCompletion
{
    public class WorkoutCompletionCommand : IRequest<Result<WorkOutCompletionResponse>>
    {
        public int WorkoutId { get; set; }
        public string UserId { get; set; } = null!;
        public string SessionId { get; set; } = null!;
        public int DurationInMinutes { get; set; }
        public int CaloriesBurned { get; set; }
        public int Rating { get; set; }
        public string? Notes { get; set; }
        public List<ExerciseCompletedDto> ExercisesCompleted { get; set; } = new List<ExerciseCompletedDto>();
    }
}
