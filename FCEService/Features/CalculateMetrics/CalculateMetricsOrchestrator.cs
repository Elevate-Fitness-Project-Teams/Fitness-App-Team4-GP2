using BuildingBlocks.Shared.Results;
using FCEService.Features.CalculateMetrics.Commands;
using FCEService.Features.CalculateMetrics.Queries;
using FCEService.Features.Shared.FCEService.Features.UserFitnessStats.Queries;
using Mapster;
using MediatR;

namespace FCEService.Features.CalculateMetrics
{
    public sealed class CalculateMetricsOrchestrator(ISender sender)
        : IRequestHandler<CalculateMetricsCommand, Result<CalculateMetricsResponse>>
    {
        public async Task<Result<CalculateMetricsResponse>> Handle(
            CalculateMetricsCommand command, CancellationToken ct)
        {
            var existingResult = await sender.Send(
                new GetCalculatedMetricByUserIdQuery(command.UserId), ct);

            if (existingResult.IsFailure)
                return Result<CalculateMetricsResponse>.Fail(existingResult.Errors);

            if (existingResult.Value is { } existing)
                return Result<CalculateMetricsResponse>.OK(existing.Adapt<CalculateMetricsResponse>());

            var statResult = await sender.Send(new GetLatestFitnessStatQuery(command.UserId), ct);

            if (statResult.IsFailure)
                return Result<CalculateMetricsResponse>.Fail(statResult.Errors);

            var stat = statResult.Value;

            var createResult = await sender.Send(
                new CreateCalculatedMetricCommand(
                    stat.UserId, stat.Weight, stat.Height, stat.Age,
                    stat.Gender, stat.ActivityLevel, stat.Goal), ct);

            return createResult.IsFailure
                ? Result<CalculateMetricsResponse>.Fail(createResult.Errors)
                : Result<CalculateMetricsResponse>.OK(createResult.Value.Adapt<CalculateMetricsResponse>());
        }
    }
}