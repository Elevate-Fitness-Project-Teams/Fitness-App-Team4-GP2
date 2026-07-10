    using BuildingBlocks.Shared.Results;
    using global::FCEService.Domain.Entities;
    using global::FCEService.Domain.Interfaces;
    using global::FCEService.Features.UserAssignedPlans.Dtos;
    using MediatR;

    namespace FCEService.Features.UserAssignedPlans.Queries
    {
        public sealed record GetActiveUserAssignedPlanQuery(Guid UserId)
            : IRequest<Result<ActiveUserAssignedPlanDto?>>;

      
        public sealed class GetActiveUserAssignedPlanHandler(IFceUnitOfWork uow)
            : IRequestHandler<GetActiveUserAssignedPlanQuery, Result<ActiveUserAssignedPlanDto?>>
        {
            public async Task<Result<ActiveUserAssignedPlanDto?>> Handle(
                GetActiveUserAssignedPlanQuery query, CancellationToken ct)
            {
                var repository = uow.GetRepository<UserAssignedPlan>(); 

                var active = await repository.FirstOrDefaultAsync(
                    a => a.UserId == query.UserId && a.IsActive, ct);

                var dto = active is null ? null
                    : new ActiveUserAssignedPlanDto(active.Id, active.UserId, active.PlanId, active.AssignedAt);

                return Result<ActiveUserAssignedPlanDto?>.OK(dto);
            }
        }
    }

