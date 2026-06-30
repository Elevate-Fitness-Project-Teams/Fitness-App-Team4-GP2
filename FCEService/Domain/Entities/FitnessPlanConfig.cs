namespace FCEService.Domain.Entities
{
    public class FitnessPlanConfig
    {
        public string PlanId { get; set; } = null!;
        public string PlanName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Goal { get; set; } = null!;
        public string Status { get; set; } = null!;
        public double MinCalorie { get; set; }
        public double MaxCalorie { get; set; }
        public string EstimatedDuration { get; set; } = null!;
        public int WorkoutsPerWeek { get; set; }
        public string ProgramType { get; set; } = null!;
    }
}
