namespace SmartCoachService.Infrastructure.Persistence.Integrations.Responses
{
    // Mirrors WorkoutService.WorkOutViewModel returned by GET /api/v1/WorkOut/GetWorkOuts.
    public record WorkOutItemResponse(
        int WorkoutId,
        string PlanId,
        string Name,
        string Category,
        int DurationInMinutes,
        string Difficulty);
}
