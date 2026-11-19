using BuildingBlocks.Shared.Controllers;
using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionService.Features.CancelSubscription;
using SubscriptionService.Features.GetSubscriptionStatus;
using SubscriptionService.Features.UpgradeToPremium;

namespace SubscriptionService.Controllers
{
    [Authorize]
    public class SubscriptionController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public SubscriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetMySubscriptionStatus()
        {
            var userId = GetCurrentUserId();
            var query = new GetSubscriptionStatusQuery(userId);
            var result = await _mediator.Send(query);
            return FromResult(result, "Subscription Status Retrieved Successfully", StatusCodes.Status200OK);
        }

        [HttpGet("status/{userId}")]
        public async Task<IActionResult> GetSubscriptionStatus([FromRoute] Guid userId)
        {
            //var userId = GetCurrentUserId();
            if (!IsOwnerOrAdmin(userId))
            {
                return FromResult(
                    Result<SubscriptionStatusResponse>.Fail(Error.Forbidden("AUTH_FORBIDDEN", "You do not have permission to view subscription status for another user.")),
                    "",
                    StatusCodes.Status403Forbidden);    
            }

            var query = new GetSubscriptionStatusQuery(userId);
            var result = await _mediator.Send(query);
            return FromResult(result, "Subscription Status Retrieved Successfully", StatusCodes.Status200OK);
        }

        [HttpPost("upgrade")]
        public async Task<IActionResult> UpgradeToPremium(UpgradeToPremiumRequest request)
        {
            var userId = GetCurrentUserId();

            var command = new UpgradeToPremiumCommand(userId, request.Tier, request.DurationMonths);
            var result = await _mediator.Send(command);
            return FromResult(result, "Subscription Upgraded Successfully", StatusCodes.Status200OK);
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelSubscription()
        {
            var userId = GetCurrentUserId();

            var command = new CancelSubscriptionCommand(userId);
            var result = await _mediator.Send(command);
            return FromResult(result, "Subscription Canceled Successfully", StatusCodes.Status200OK);
        }
    }
}
