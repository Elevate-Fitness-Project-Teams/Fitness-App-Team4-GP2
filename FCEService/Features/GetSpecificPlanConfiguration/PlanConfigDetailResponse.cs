namespace FCEService.Features.GetSpecificPlanConfiguration
{
  
        public sealed record PlanConfigDetailResponse(
            string PlanId, string PlanName, string Description, string Goal, string Status,
            double MinCalorie, double MaxCalorie, string EstimatedDuration,
            int WorkoutsPerWeek, string ProgramType);
  
}
