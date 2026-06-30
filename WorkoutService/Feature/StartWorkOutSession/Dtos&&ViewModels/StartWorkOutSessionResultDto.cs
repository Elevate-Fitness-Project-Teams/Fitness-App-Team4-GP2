using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;

namespace WorkoutService.Feature.StartWorkOutSession.Dtos__ViewModels
{
    public class StartWorkOutSessionResultDto
    {
        public string SessionId { get; set; } = null!;
        public WorkOutSessionStatus Status { get; set; }

        public ICollection<WorkoutExerciseInSessionDto> ExercisesInSession { get; set; } = new List<WorkoutExerciseInSessionDto>();


    }
}
