namespace SmartCoachService.Infrastructure.Persistence.Integrations.Responses
{
    public record FitnessMetricsResponse(
        Guid UserId,
        double Bmr,
        double Tdee,
        double CalorieTarget,
        string Status,
        DateTime CalculatedAt);

}
