using FCEService.Domain.Enums;

namespace FCEService.Features.UserAssignedPlans.Dtos
{
    public sealed record FitnessPlanConfigDto(
             string PlanId, string PlanName, string Description, FitnessGoal Goal, PlanStatus Status,
             double MinCalorie, double MaxCalorie, string EstimatedDuration, int WorkoutsPerWeek, string ProgramType);

}
