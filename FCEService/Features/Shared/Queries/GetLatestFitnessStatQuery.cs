namespace FCEService.Features.Shared
{
    using BuildingBlocks.Shared.Results;
    using global::FCEService.Common;
    using global::FCEService.Domain.Entities;
    using global::FCEService.Domain.Enums;
    using global::FCEService.Domain.Interfaces;
    using global::FCEService.Features.Shared.Dtos;
    using MediatR;

    namespace FCEService.Features.UserFitnessStats.Queries
    {
        public sealed record GetLatestFitnessStatQuery(Guid UserId)
            : IRequest<Result<UserFitnessStatDto>>;

        public sealed class GetLatestFitnessStatHandler(IFceUnitOfWork uow)
            : IRequestHandler<GetLatestFitnessStatQuery, Result<UserFitnessStatDto>>
        {
            public async Task<Result<UserFitnessStatDto>> Handle(
                GetLatestFitnessStatQuery query, CancellationToken ct)
            {
                var repository = uow.GetRepository<UserFitnessStat>(); 

                var stat = await repository.FirstOrderedAsync(
                    s => s.UserId == query.UserId,
                    q => q.OrderByDescending(s => s.RecordedAt),
                    ct);

               
                return stat is null
                    ? Result<UserFitnessStatDto>.Fail(FceErrors.StatsNotFound)
                    : Result<UserFitnessStatDto>.OK(new UserFitnessStatDto
                    (
                        stat.UserId, 
                        stat.Weight,
                        stat.Height, 
                        stat.Age,
                        stat.Gender,
                        stat.ActivityLevel,
                        stat.Goal,
                        stat.RecordedAt
                    )
                    );
            }
        }
    }
}
