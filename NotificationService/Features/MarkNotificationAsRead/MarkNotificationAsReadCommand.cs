using BuildingBlocks.Shared.Results;
using FluentValidation;
using NotificationService.Common;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces;
using MediatR;

namespace NotificationService.Features.MarkNotificationAsRead
{
    public sealed record MarkNotificationAsReadCommand(int Id, Guid UserId) 
        : IRequest<Result<MarkNotificationAsReadResponse>>;

    public sealed record MarkNotificationAsReadResponse(int Id, bool IsRead);

    public sealed class MarkNotificationAsReadValidator : AbstractValidator<MarkNotificationAsReadCommand>
    {
        public MarkNotificationAsReadValidator()
        {
            RuleFor(c => c.Id).GreaterThan(0).WithMessage("A valid notification Id is required.");
        }
    }

    public sealed class MarkNotificationAsReadHandler(INotificationUnitOfWork uow)
        : IRequestHandler<MarkNotificationAsReadCommand, Result<MarkNotificationAsReadResponse>>
    {
        public async Task<Result<MarkNotificationAsReadResponse>> Handle(
            MarkNotificationAsReadCommand command, CancellationToken ct)
        {
            var repository = uow.GetRepository<InAppNotification>();

            var exists = await repository.ExistsAsync(
                n => n.Id == command.Id && n.UserId == command.UserId, ct);

            if (!exists)
                return Result<MarkNotificationAsReadResponse>.Fail(NotificationErrors.NotFound);

            var stub = new InAppNotification { Id = command.Id, IsRead = true };
            repository.SaveInclude(stub, nameof(InAppNotification.IsRead));
            await uow.SaveChangesAsync(ct);

            return Result<MarkNotificationAsReadResponse>.OK(
                new MarkNotificationAsReadResponse(command.Id, true));
        }
    }
}