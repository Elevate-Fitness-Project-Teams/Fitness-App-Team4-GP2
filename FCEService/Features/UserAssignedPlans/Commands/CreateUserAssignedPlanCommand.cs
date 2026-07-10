using BuildingBlocks.Shared.Results;
using FCEService.Domain.Entities;
using FCEService.Domain.Interfaces;
using MediatR;

namespace FCEService.Features.UserAssignedPlans.Commands
{
    public sealed record CreateUserAssignedPlanCommand(Guid UserId, string PlanId) : IRequest<Result<DateTime>>;

    public sealed class CreateUserAssignedPlanHandler(IFceUnitOfWork uow)
        : IRequestHandler<CreateUserAssignedPlanCommand, Result<DateTime>>
    {
        public async Task<Result<DateTime>> Handle(CreateUserAssignedPlanCommand command, CancellationToken ct)
        {
            var repository = uow.GetRepository<UserAssignedPlan>(); 

            var assignment = new UserAssignedPlan
            {
                UserId = command.UserId,
                PlanId = command.PlanId,
                AssignedAt = DateTime.UtcNow,
                IsActive = true
            };

            await repository.AddAsync(assignment, ct);
            await uow.SaveChangesAsync(ct);

            return Result<DateTime>.OK(assignment.AssignedAt);
        }
    }
}
