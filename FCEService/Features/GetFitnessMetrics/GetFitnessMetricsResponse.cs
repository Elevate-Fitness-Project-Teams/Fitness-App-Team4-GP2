namespace FCEService.Features.GetFitnessMetrics
{
    // RESPONSE DTO — matches docs: bmr / tdee / calorieTarget / status / calculatedAt
    // ══════════════════════════════════════════════════════════════════
    public sealed record GetFitnessMetricsResponse(
        Guid UserId,
        double Bmr,
        double Tdee,
        double CalorieTarget,
        string Status,
        DateTime CalculatedAt
    );
}
