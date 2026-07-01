using BuildingBlocks.Shared.Results;
using FCEService.Domain.Entities;
using FCEService.Domain.Enums;
using MediatR;

namespace FCEService.Features.CalculateMetrics
{
    public sealed record UpsertCalculatedMetricCommand(
         Guid UserId,
         double Bmr,
         double Tdee,
         double CalorieTarget,
         PlanStatus Status
     ) : IRequest<Result<CalculatedMetric>>;
}
