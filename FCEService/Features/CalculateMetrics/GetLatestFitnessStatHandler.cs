using BuildingBlocks.Shared.Results;
using FCEService.Common;
using FCEService.Domain.Entities;
using FCEService.Domain.Interfaces;
using MediatR;

namespace FCEService.Features.CalculateMetrics
{
    public sealed class GetLatestFitnessStatHandler(IFceUnitOfWork uow)
          : IRequestHandler<GetLatestFitnessStatQuery, Result<UserFitnessStat>>
    {
        public async Task<Result<UserFitnessStat>> Handle(
            GetLatestFitnessStatQuery query,
            CancellationToken ct)
        {
            var stat = await uow.GetRepository<UserFitnessStat>().FirstOrderedAsync(
                s => s.UserId == query.UserId,
                q => q.OrderByDescending(s => s.RecordedAt),
                ct);

            return stat is null
                ? Result<UserFitnessStat>.Fail(FceErrors.StatsNotFound)
                : Result<UserFitnessStat>.OK(stat);
        }
    }
}
