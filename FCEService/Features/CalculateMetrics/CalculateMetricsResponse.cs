namespace FCEService.Features.CalculateMetrics
{
    public sealed record CalculateMetricsResponse(
        Guid UserId,
        double Bmr,
        double Tdee,
        double CalorieTarget,
        string Status,
        DateTime CalculatedAt,
        DateTime? LastUpdatedAt);
}
