using BuildingBlocks.Shared.Results;
using FCEService.Common;
using FCEService.Domain.Entities;
using FCEService.Domain.Interfaces;
using MediatR;

namespace FCEService.Features.GetFitnessMetrics
{
    public sealed class GetFitnessMetricsHandler(IFceUnitOfWork uow)
        : IRequestHandler<GetFitnessMetricsQuery, Result<GetFitnessMetricsResponse>>
    {
        public async Task<Result<GetFitnessMetricsResponse>> Handle(
            GetFitnessMetricsQuery query,
            CancellationToken ct)
        {
            var metric = await uow.GetRepository<CalculatedMetric>().FirstOrDefaultAsync(
                m => m.UserId == query.UserId, ct);

            if (metric is null)
                return Result<GetFitnessMetricsResponse>.Fail(FceErrors.MetricsNotCalculated);

            return Result<GetFitnessMetricsResponse>.OK(new GetFitnessMetricsResponse(
                metric.UserId,
                metric.Bmr,
                metric.Tdee,
                metric.CalorieTarget,
                metric.Status.ToString(),
                metric.CalculatedAt));
        }
    }
}
