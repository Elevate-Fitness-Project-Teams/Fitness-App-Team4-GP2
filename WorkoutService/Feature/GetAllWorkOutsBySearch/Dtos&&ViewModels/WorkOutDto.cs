using WorkoutService.Domain.Enums;

namespace WorkoutService.Feature.GetAllWorkOuts.Dtos
{
    public class WorkOutDto
    {
        public int WorkoutId { get; set; }
        public string PlanId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public WorkOutCategory Category { get; set; }
        public int DurationInMinutes { get; set; }
        public WorkOutDiffeculty Difficulty { get; set; }
    }
}
