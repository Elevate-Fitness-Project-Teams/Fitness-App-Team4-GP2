    using BuildingBlocks.Shared.Results;
    using FCEService.Domain.Entities;
    using FCEService.Domain.Interfaces;
    using MediatR;

namespace FCEService.Features.UserAssignedPlans.Commands
{
    public sealed record DeactivateUserAssignedPlanCommand(
            int Id, Guid UserId, string PlanId, DateTime AssignedAt) : IRequest<Result>;

        public sealed class DeactivateUserAssignedPlanHandler(IFceUnitOfWork uow)
            : IRequestHandler<DeactivateUserAssignedPlanCommand, Result>
        {
            public async Task<Result> Handle(DeactivateUserAssignedPlanCommand command, CancellationToken ct)
            {
                var repository = uow.GetRepository<UserAssignedPlan>(); 

                var stub = new UserAssignedPlan
                {
                    Id = command.Id,
                    UserId = command.UserId,
                    PlanId = command.PlanId,
                    AssignedAt = command.AssignedAt,
                    IsActive = false
                };

                repository.SaveInclude(stub, nameof(UserAssignedPlan.IsActive));
                await uow.SaveChangesAsync(ct); 
                return Result.OK();
            }
        }
    
}
