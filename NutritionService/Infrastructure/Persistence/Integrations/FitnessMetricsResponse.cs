namespace NutritionService.Infrastructure.Persistence.Integrations
{
    public record FitnessMetricsResponse(
        Guid UserId,
        double Bmr,
        double Tdee,
        double CalorieTarget,
        string Status,
        DateTime CalculatedAt);
    
}