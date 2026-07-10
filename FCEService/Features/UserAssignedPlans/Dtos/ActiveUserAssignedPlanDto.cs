namespace FCEService.Features.UserAssignedPlans.Dtos
{
    public sealed record ActiveUserAssignedPlanDto
        (int Id, Guid UserId, string PlanId, DateTime AssignedAt);

}
