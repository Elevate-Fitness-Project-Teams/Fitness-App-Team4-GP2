namespace FCEService.Features.GetFitnessMetrics
{
    
    public sealed record GetFitnessMetricsResponse(
        Guid UserId,
        double Bmr,
        double Tdee,
        double CalorieTarget,
        string Status,
        DateTime CalculatedAt
    );
}
