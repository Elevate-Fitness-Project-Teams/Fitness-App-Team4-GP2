using BuildingBlocks.Shared.Results;
using FCEService.Common;
using FCEService.Domain.Entities;
using FCEService.Domain.Enums;
using FCEService.Domain.Interfaces;
using FCEService.Domain.Services;
using MediatR;

namespace FCEService.Features.CalculateMetrics
{

    public sealed class CalculateMetricsHandler(
        ISender sender,
        IMetabolicCalculatorService calculator)
        : IRequestHandler<CalculateMetricsCommand, Result<CalculateMetricsResponse>>
    {
        public async Task<Result<CalculateMetricsResponse>> Handle(
            CalculateMetricsCommand command,
            CancellationToken ct)
        {

            var statResult = await sender.Send(
                new GetLatestFitnessStatQuery(command.UserId), ct);

            if (statResult.IsFailure)
                return Result<CalculateMetricsResponse>.Fail(statResult.Errors[0]);

            var stat = statResult.Value;

          
            var bmr = calculator.CalculateBmr(stat.Weight, stat.Height, stat.Age, stat.Gender);
            var tdee = calculator.CalculateTdee(bmr, stat.ActivityLevel);
            var calorieTarget = calculator.CalculateCalorieTarget(tdee, stat.Goal);
            var status = calculator.DetermineStatus(calorieTarget);

         
            if (!double.IsFinite(bmr) || !double.IsFinite(tdee) ||
                !double.IsFinite(calorieTarget) || status == PlanStatus.None)
                return Result<CalculateMetricsResponse>.Fail(FceErrors.InvalidCalculation);

         
            var saveResult = await sender.Send(
                new UpsertCalculatedMetricCommand(command.UserId, bmr, tdee, calorieTarget, status), ct);

            if (saveResult.IsFailure)
                return Result<CalculateMetricsResponse>.Fail(saveResult.Errors[0]);

            var metric = saveResult.Value;

            return Result<CalculateMetricsResponse>.OK(new CalculateMetricsResponse(
                metric.UserId,
                metric.Bmr,
                metric.Tdee,
                metric.CalorieTarget,
                metric.Status.ToString(),
                metric.CalculatedAt,
                metric.LastUpdatedAt));
        }
    }
}