namespace FCEService.Features.GetFitnessPlanConfigs
{
    namespace FCEService.Features.GetFitnessPlanConfigs
    {
        public sealed record FitnessPlanConfigResponse(
            string PlanId, string PlanName, string Description, string Goal, string Status,
            double MinCalorie, double MaxCalorie, string EstimatedDuration,
            int WorkoutsPerWeek, string ProgramType);
    }
}
