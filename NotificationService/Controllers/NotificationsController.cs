using System.Security.Claims;
using BuildingBlocks.Shared.Controllers;
using BuildingBlocks.Shared.Exceptions;
using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Features.GetNotifications;
using NotificationService.Features.MarkNotificationAsRead;

namespace NotificationService.Controllers
{
    [Authorize]
    [Route("api/v1/notifications")]
    public sealed class NotificationsController(ISender sender) : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetNotifications(CancellationToken ct)
        {
            var result = await sender.Send(new GetNotificationsQuery(GetCurrentUserId()), ct);
            return FromResult(result, "Notifications fetched successfully.", StatusCodes.Status200OK);
        }

        [HttpPut("{id:int}/read")]
        public async Task<IActionResult> MarkAsRead(int id, CancellationToken ct)
        {
            var result = await sender.Send(new MarkNotificationAsReadCommand(id, GetCurrentUserId()), ct);
            return FromResult(result, "Notification marked as read.", StatusCodes.Status200OK);
        }

        
        private Guid GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            if (claim is null || !Guid.TryParse(claim.Value, out var userId))
                throw new AppException(Error.Unauthorized("AUTH_INVALID_TOKEN", "Unable to resolve the authenticated user."));

            return userId;
        }

    }
}