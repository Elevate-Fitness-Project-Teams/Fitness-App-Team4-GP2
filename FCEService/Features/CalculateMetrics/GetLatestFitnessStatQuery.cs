using BuildingBlocks.Shared.Results;
using FCEService.Domain.Entities;
using MediatR;

namespace FCEService.Features.CalculateMetrics
{
    public sealed record GetLatestFitnessStatQuery(Guid UserId)
         : IRequest<Result<UserFitnessStatDto>>;
}
