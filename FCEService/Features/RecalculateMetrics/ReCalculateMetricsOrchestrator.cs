using BuildingBlocks.Shared.Results;
using FCEService.Common;
using FCEService.Domain.Interfaces;
using FCEService.Features.FitnessPlanConfigs.Queries;
using FCEService.Features.Shared.FCEService.Features.UserFitnessStats.Queries;
using FCEService.Features.Shared.Queries;
using FCEService.Features.UserAssignedPlans.Commands;
using FCEService.Features.UserAssignedPlans.Queries;
using MediatR;

namespace FCEService.Features.RecalculateMetrics
{ 
        public sealed record RecalculateMetricsCommand(
            Guid UserId, string? Reason, double? NewWeight, string? TriggeredBy)
            : IRequest<Result<RecalculateMetricsResponse>>;

        public sealed class RecalculateMetricsOrchestrator(ISender sender, IFceUnitOfWork uow)
            : IRequestHandler<RecalculateMetricsCommand, Result<RecalculateMetricsResponse>>
        {
            public async Task<Result<RecalculateMetricsResponse>> Handle(
                RecalculateMetricsCommand command, CancellationToken ct)
            {
                var metricResult = await sender.Send(new GetCalculatedMetricByUserIdQuery(command.UserId), ct);
                if (metricResult.IsFailure)
                    return Result<RecalculateMetricsResponse>.Fail(metricResult.Errors);

                if (metricResult.Value is not { } existingMetric)
                    return Result<RecalculateMetricsResponse>.Fail(FceErrors.MetricsNotCalculated);

                var statResult = await sender.Send(new GetLatestFitnessStatQuery(command.UserId), ct);
                if (statResult.IsFailure)
                    return Result<RecalculateMetricsResponse>.Fail(FceErrors.MetricsNotCalculated);

                var stat = statResult.Value;

            
                var weightForCalculation = command.NewWeight ?? stat.Weight;

            

                var updateMetricResult = await sender.Send(
                    new UpdateCalculatedMetricCommand(
                        existingMetric.Id, command.UserId, weightForCalculation, stat.Height, stat.Age,
                        stat.Gender, stat.ActivityLevel, stat.Goal, existingMetric.CalculatedAt), ct);

                if (updateMetricResult.IsFailure)
                {
             
                    return Result<RecalculateMetricsResponse>.Fail(updateMetricResult.Errors);
                }

                var newMetric = updateMetricResult.Value;
                var planReassignment = newMetric.Status != existingMetric.Status;

                if (planReassignment)
                {
                    var planConfigResult = await sender.Send(
                        new GetMatchingFitnessPlanConfigQuery(stat.Goal, newMetric.Status), ct);

                    if (planConfigResult.IsFailure)
                    {
                     
                        return Result<RecalculateMetricsResponse>.Fail(planConfigResult.Errors);
                    }

                    if (planConfigResult.Value is not { } matchingPlan)
                    {
                      
                        return Result<RecalculateMetricsResponse>.Fail(
                            FceErrors.NoMatchingPlan(stat.Goal.ToString(), newMetric.Status.ToString()));
                    }

                    var priorActiveResult = await sender.Send(new GetActiveUserAssignedPlanQuery(command.UserId), ct);
                    if (priorActiveResult.IsFailure)
                    {
                   
                        return Result<RecalculateMetricsResponse>.Fail(priorActiveResult.Errors);
                    }

                    if (priorActiveResult.Value is { } priorActive)
                    {
                        var deactivateResult = await sender.Send(
                            new DeactivateUserAssignedPlanCommand(
                                priorActive.Id, priorActive.UserId, priorActive.PlanId, priorActive.AssignedAt), ct);

                        if (deactivateResult.IsFailure)
                        {
                          
                            return Result<RecalculateMetricsResponse>.Fail(deactivateResult.Errors);
                        }

                      
                        var historyResult = await sender.Send(
                            new AppendUserPlanHistoryCommand(
                                priorActive.UserId, priorActive.PlanId, priorActive.AssignedAt,
                                DateTime.UtcNow,command.Reason), ct);

                        if (historyResult.IsFailure)
                        {
                            return Result<RecalculateMetricsResponse>.Fail(historyResult.Errors);
                        }
                    }

                    var createPlanResult = await sender.Send(
                        new CreateUserAssignedPlanCommand(command.UserId, matchingPlan.PlanId), ct);

                    if (createPlanResult.IsFailure)
                    {
                     
                        return Result<RecalculateMetricsResponse>.Fail(createPlanResult.Errors);
                    }
                }

                var response = new RecalculateMetricsResponse(
                    command.UserId,
                    new MetricsSnapshot(existingMetric.Bmr, existingMetric.Tdee, existingMetric.CalorieTarget, existingMetric.Status.ToString()),
                    new MetricsSnapshot(newMetric.Bmr, newMetric.Tdee, newMetric.CalorieTarget, newMetric.Status.ToString()),
                    planReassignment);

                return Result<RecalculateMetricsResponse>.OK(response);
            }
        }
}
