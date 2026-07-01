using BuildingBlocks.Shared.Results;
using FCEService.Common;
using FCEService.Domain.Entities;
using FCEService.Domain.Interfaces;
using MediatR;

namespace FCEService.Features.GetFitnessStats
{
    public sealed class GetFitnessStatsHandler(IFceUnitOfWork uow)
         : IRequestHandler<GetFitnessStatsQuery, Result<GetFitnessStatsResponse>>
    {
        public async Task<Result<GetFitnessStatsResponse>> Handle(
            GetFitnessStatsQuery query,
            CancellationToken ct)
        {
            var stat = await uow.GetRepository<UserFitnessStat>().FirstOrderedAsync(
                s => s.UserId == query.UserId,
                q => q.OrderByDescending(s => s.RecordedAt),
                ct);

            if (stat is null)
                return Result<GetFitnessStatsResponse>.Fail(FceErrors.StatsNotFound);

            return Result<GetFitnessStatsResponse>.OK(new GetFitnessStatsResponse(
                stat.UserId,
                stat.Weight,
                stat.Height,
                stat.Age,
                stat.Gender.ToString(),
                stat.Goal.ToString(),
                stat.ActivityLevel.ToString(),
                stat.RecordedAt));
        }
    }
}