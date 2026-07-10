using BuildingBlocks.Shared.Results;
using FCEService.Common;
using FCEService.Domain.Interfaces;
using FCEService.Features.FitnessPlanConfigs.Queries;
using FCEService.Features.Shared.FCEService.Features.UserFitnessStats.Queries;
using FCEService.Features.Shared.Queries;
using FCEService.Features.UserAssignedPlans.Commands;
using FCEService.Features.UserAssignedPlans.Queries;
using Mapster;
using MediatR;

namespace FCEService.Features.UserAssignedPlans
{
        public sealed record AssignFitnessPlanCommand(Guid UserId) : IRequest<Result<AssignFitnessPlanResponse>>;
        public sealed class AssignFitnessPlanOrchestrator(ISender sender)
            : IRequestHandler<AssignFitnessPlanCommand, Result<AssignFitnessPlanResponse>>
        {
            public async Task<Result<AssignFitnessPlanResponse>> Handle(
                AssignFitnessPlanCommand command, CancellationToken ct)
            {
            // Read (Queries)
            var metricResult = await sender.Send(new GetCalculatedMetricByUserIdQuery(command.UserId), ct);
                if (metricResult.IsFailure)
                    return Result<AssignFitnessPlanResponse>.Fail(metricResult.Errors);

                if (metricResult.Value is not { } metric)
                    return Result<AssignFitnessPlanResponse>.Fail(FceErrors.MetricsNotCalculated);

                var statResult = await sender.Send(new GetLatestFitnessStatQuery(command.UserId), ct);
                if (statResult.IsFailure)
                    return Result<AssignFitnessPlanResponse>.Fail(statResult.Errors);

                var planConfigResult = await sender.Send(
                    new GetMatchingFitnessPlanConfigQuery(statResult.Value.Goal, metric.Status), ct);
                if (planConfigResult.IsFailure)
                    return Result<AssignFitnessPlanResponse>.Fail(planConfigResult.Errors);

                if (planConfigResult.Value is not { } matchingPlan)
                    return Result<AssignFitnessPlanResponse>.Fail(
                        FceErrors.NoMatchingPlan(statResult.Value.Goal.ToString(), metric.Status.ToString()));

                var priorActiveResult = await sender.Send(new GetActiveUserAssignedPlanQuery(command.UserId), ct);
                if (priorActiveResult.IsFailure)
                    return Result<AssignFitnessPlanResponse>.Fail(priorActiveResult.Errors);

                var priorActive = priorActiveResult.Value;

                if (priorActive is not null && priorActive.PlanId == matchingPlan.PlanId)
                    return Result<AssignFitnessPlanResponse>.Fail(FceErrors.PlanAlreadyAssigned);

                ////Write (Transaction )
                //await uow.BeginTransactionAsync(ct);

                if (priorActive is not null)
                {
                    var deactivateResult = await sender.Send(
                        new DeactivateUserAssignedPlanCommand(
                            priorActive.Id, priorActive.UserId, priorActive.PlanId, priorActive.AssignedAt), ct);

                    if (deactivateResult.IsFailure)
                    {
                        //await uow.RollbackTransactionAsync(ct);
                        return Result<AssignFitnessPlanResponse>.Fail(deactivateResult.Errors);
                    }

                    var historyResult = await sender.Send(
                        new AppendUserPlanHistoryCommand(
                            priorActive.UserId, priorActive.PlanId, priorActive.AssignedAt,
                            DateTime.UtcNow, "Reassigned due to updated goal/status match"), ct);

                    if (historyResult.IsFailure)
                    {
                        //await uow.RollbackTransactionAsync(ct);
                        return Result<AssignFitnessPlanResponse>.Fail(historyResult.Errors);
                    }
                }

                var createResult = await sender.Send(
                    new CreateUserAssignedPlanCommand(command.UserId, matchingPlan.PlanId), ct);

                if (createResult.IsFailure)
                {
                    //await uow.RollbackTransactionAsync(ct);
                    return Result<AssignFitnessPlanResponse>.Fail(createResult.Errors);
                }

                //await uow.CommitTransactionAsync(ct);

                var response = matchingPlan.Adapt<AssignFitnessPlanResponse>() with
                {
                    AssignedAt = createResult.Value
                };

                return Result<AssignFitnessPlanResponse>.OK(response);
            }
        }
}


