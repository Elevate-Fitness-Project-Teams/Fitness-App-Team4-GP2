using BuildingBlocks.Shared.Results;
using MediatR;

namespace FCEService.Features.GetFitnessMetrics
{
    public sealed record GetFitnessMetricsQuery(Guid UserId)
        : IRequest<Result<GetFitnessMetricsResponse>>;
}
