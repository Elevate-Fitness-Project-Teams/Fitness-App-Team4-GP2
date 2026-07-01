namespace FCEService.Features.CalculateMetrics
{
    // RESPONSE DTO
    public sealed record CalculateMetricsResponse(
       Guid UserId,
       double Bmr,
       double Tdee,
       double CalorieTarget,
       string Status,
       DateTime CalculatedAt,
       DateTime? LastUpdatedAt
   );
}
