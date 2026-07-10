using BuildingBlocks.Shared.Results;
using FCEService.Domain.Entities;
using FCEService.Domain.Interfaces;
using MediatR;

namespace FCEService.Features.UserAssignedPlans.Commands
{
    public sealed record AppendUserPlanHistoryCommand(
         Guid UserId, string PlanId, DateTime AssignedAt, DateTime? EndedAt, string ReasonForChange)
         : IRequest<Result>;

    public sealed class AppendUserPlanHistoryHandler(IFceUnitOfWork uow)
        : IRequestHandler<AppendUserPlanHistoryCommand, Result>
    {
        public async Task<Result> Handle(AppendUserPlanHistoryCommand command, CancellationToken ct)
        {
            var repository = uow.GetRepository<UserPlanHistory>(); 

            var entry = new UserPlanHistory
            {
                UserId = command.UserId,
                PlanId = command.PlanId,
                AssignedAt = command.AssignedAt,
                EndedAt = command.EndedAt,
                ReasonForChange = command.ReasonForChange
            };

            await repository.AddAsync(entry, ct);
            await uow.SaveChangesAsync(ct);

            return Result.OK();
        }
    }
}
