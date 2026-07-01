using BuildingBlocks.Shared.Results;
using MediatR;

namespace FCEService.Features.GetFitnessStats
{
    public sealed record GetFitnessStatsQuery(Guid UserId)
         : IRequest<Result<GetFitnessStatsResponse>>;
}
