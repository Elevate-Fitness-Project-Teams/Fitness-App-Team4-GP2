using FCEService.Domain.Enums;

namespace FCEService.Features.CalculateMetrics.Queries
{
    public sealed record CalculatedMetricDto(
       Guid UserId,
       double Bmr,
       double Tdee,
       double CalorieTarget,
       PlanStatus Status,
       DateTime CalculatedAt,
       DateTime? LastUpdatedAt
   );
}
