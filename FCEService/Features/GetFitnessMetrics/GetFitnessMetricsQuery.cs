using BuildingBlocks.Shared.Results;
using FCEService.Common;
using FCEService.Domain.Entities;
using FCEService.Domain.Interfaces;
using FCEService.Features.Shared.Queries;
using Mapster;
using MediatR;

namespace FCEService.Features.GetFitnessMetrics
{
    public sealed record GetFitnessMetricsQuery(Guid UserId)
        : IRequest<Result<GetFitnessMetricsResponse>>;
    public sealed class GetFitnessMetricsQueryHandler(ISender sender)
        : IRequestHandler<GetFitnessMetricsQuery, Result<GetFitnessMetricsResponse>>
    {
        public async Task<Result<GetFitnessMetricsResponse>> Handle(
            GetFitnessMetricsQuery query, CancellationToken ct)
        {
            var result = await sender.Send(new GetCalculatedMetricByUserIdQuery(query.UserId), ct);

            if (result.IsFailure)
                return Result<GetFitnessMetricsResponse>.Fail(result.Errors);

            
            return result.Value is null
                ? Result<GetFitnessMetricsResponse>.Fail(FceErrors.MetricsNotCalculated)
                : Result<GetFitnessMetricsResponse>.OK(result.Value.Adapt<GetFitnessMetricsResponse>());
        }
    }
}

