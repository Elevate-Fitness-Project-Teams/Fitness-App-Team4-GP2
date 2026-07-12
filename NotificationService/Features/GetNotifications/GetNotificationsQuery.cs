using BuildingBlocks.Shared.Results;
using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces;
using MediatR;

namespace NotificationService.Features.GetNotifications
{
    public sealed record GetNotificationsQuery(Guid UserId) 
        : IRequest<Result<IReadOnlyList<NotificationResponse>>>;

    public sealed record NotificationResponse(int Id, string Title, string Message, bool IsRead);

    public sealed class GetNotificationsHandler(INotificationUnitOfWork uow)
        : IRequestHandler<GetNotificationsQuery, Result<IReadOnlyList<NotificationResponse>>>
    {
        public async Task<Result<IReadOnlyList<NotificationResponse>>> Handle(
            GetNotificationsQuery query, CancellationToken ct)
        {
            var notifications = await uow.GetRepository<InAppNotification>()
                .Query()
                .Where(n => n.UserId == query.UserId && !n.IsRead)
                .OrderByDescending(n => n.SentAt)
                .Select(n => new NotificationResponse(n.Id, n.Title, n.Message, n.IsRead))
                .ToListAsync(ct);

            return Result<IReadOnlyList<NotificationResponse>>.OK(notifications);
        }
    }



}