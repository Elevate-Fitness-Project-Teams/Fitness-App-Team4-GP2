using FCEService.Domain.Enums;

namespace FCEService.Features.Shared.Dtos
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
