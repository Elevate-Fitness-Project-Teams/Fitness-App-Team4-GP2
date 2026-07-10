namespace FCEService.Features.UserAssignedPlans
{
   
        public sealed record AssignFitnessPlanResponse(
            string PlanId, string PlanName, string Description, string Goal, string Status,
            double MinCalorie, double MaxCalorie, string EstimatedDuration,
            int WorkoutsPerWeek, string ProgramType, DateTime AssignedAt);
    
}
