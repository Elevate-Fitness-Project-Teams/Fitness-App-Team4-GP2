using BuildingBlocks.Shared.Results;
using FCEService.Common;
using FCEService.Domain.Entities;
using FCEService.Domain.Interfaces;
using FCEService.Features.Shared.FCEService.Features.UserFitnessStats.Queries;
using Mapster;
using MediatR;

namespace FCEService.Features.GetFitnessStats
{
    public sealed record GetFitnessStatsQuery(Guid UserId)
        : IRequest<Result<GetFitnessStatsResponse>>;
    public sealed class GetFitnessStatsQueryHandler(ISender sender)
         : IRequestHandler<GetFitnessStatsQuery, Result<GetFitnessStatsResponse>>
    {
        public async Task<Result<GetFitnessStatsResponse>> Handle(
            GetFitnessStatsQuery query,
            CancellationToken ct)
        {
            var result = await sender.Send(new GetLatestFitnessStatQuery(query.UserId), ct);

            return result.IsFailure
                ? Result<GetFitnessStatsResponse>.Fail(result.Errors)
                : Result<GetFitnessStatsResponse>.OK(result.Value.Adapt<GetFitnessStatsResponse>());
        }
    }
}