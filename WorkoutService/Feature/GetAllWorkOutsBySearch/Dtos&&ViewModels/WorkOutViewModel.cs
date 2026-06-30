using WorkoutService.Domain.Enums;

namespace WorkoutService.Feature.GetAllWorkOuts.Dtos__ViewModels
{
    public class WorkOutViewModel
    {
        public int WorkoutId { get; set; }
        public string PlanId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public int DurationInMinutes { get; set; }
        public string Difficulty { get; set; } = null!;
    }
}
