using BuildingBlocks.Shared.Results;
using MediatR;

namespace FCEService.Features.CalculateMetrics
{
    public sealed record CalculateMetricsCommand(Guid UserId)
        : IRequest<Result<CalculateMetricsResponse>>;

}
