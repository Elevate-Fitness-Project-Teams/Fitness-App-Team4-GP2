    using BuildingBlocks.Shared.Results;
    using global::FCEService.Domain.Enums;
    using global::FCEService.Domain.Interfaces;
    using global::FCEService.Features.UserAssignedPlans.Dtos;
    using MediatR;

    namespace FCEService.Features.FitnessPlanConfigs.Queries
    {
        public sealed record GetMatchingFitnessPlanConfigQuery(FitnessGoal Goal, PlanStatus Status)
            : IRequest<Result<FitnessPlanConfigDto?>>;

       
        public sealed class GetMatchingFitnessPlanConfigHandler(IFceUnitOfWork uow)
            : IRequestHandler<GetMatchingFitnessPlanConfigQuery, Result<FitnessPlanConfigDto?>>
        {
            public async Task<Result<FitnessPlanConfigDto?>> Handle(
                GetMatchingFitnessPlanConfigQuery query, CancellationToken ct)
            {
                var config = await uow.FitnessPlanConfigs.FindMatchingPlanAsync(query.Goal,query.Status,ct); 

                var dto = config is null ? null : new FitnessPlanConfigDto(
                    config.PlanId, config.PlanName, config.Description, config.Goal, config.Status,
                    config.MinCalorie, config.MaxCalorie, config.EstimatedDuration,
                    config.WorkoutsPerWeek, config.ProgramType);

                return Result<FitnessPlanConfigDto?>.OK(dto);
            }
        }
    }

