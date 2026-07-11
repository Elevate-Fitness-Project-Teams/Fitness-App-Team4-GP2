using BuildingBlocks.Shared.Results;
using FCEService.Domain.Entities;
using FCEService.Domain.Interfaces;
using FCEService.Features.Shared.Dtos;
using MediatR;

namespace FCEService.Features.Shared.Queries
{
    public sealed record GetCalculatedMetricByUserIdQuery(Guid UserId)
          : IRequest<Result<CalculatedMetricDto?>>;

    public sealed class GetCalculatedMetricByUserIdQueryHandler(IFceUnitOfWork uow)
       : IRequestHandler<GetCalculatedMetricByUserIdQuery, Result<CalculatedMetricDto?>>
    {
        public async Task<Result<CalculatedMetricDto?>> Handle(
            GetCalculatedMetricByUserIdQuery query, CancellationToken ct)
        {
            var repository = uow.GetRepository<CalculatedMetric>(); 

            var metric = await repository.FirstOrDefaultAsync(m => m.UserId == query.UserId, ct);

            var dto = metric is null ? null : new CalculatedMetricDto(
                metric.Id
               ,metric.UserId, metric.Bmr, metric.Tdee, metric.CalorieTarget,
                metric.Status, metric.CalculatedAt, metric.LastUpdatedAt);

            return Result<CalculatedMetricDto?>.OK(dto);
        }
    }
}
