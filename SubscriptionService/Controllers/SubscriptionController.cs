using BuildingBlocks.Shared.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionService.Features.CancelSubscription;
using SubscriptionService.Features.UpgradeToPremium;
using System.Security.Claims;

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
        protected Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.Parse(userIdClaim!);
        }

        [HttpGet("status/{userId}")]
        public async Task<IActionResult> GetSubscriptionStatus([FromRoute]Guid userId)
        {
            var query = new Features.GetSubscriptionStatus.GetSubscriptionStatusQuery(userId);
            var result = await _mediator.Send(query);
            return FromResult(result, "Subscription Status Retrieved Successfully", StatusCodes.Status200OK);
        }

        [HttpPost("upgrade")]
        public async Task<IActionResult> UpgradeToPremium(UpgradeToPremiumRequest request)
        {
            var userId = GetCurrentUserId();
            //var userId = new Guid("00000000-0000-0000-0000-000000000001");

            var command = new UpgradeToPremiumCommand(userId, request.Tier, request.DurationMonths);
            var result = await _mediator.Send(command);
            return FromResult(result, "Subscription Upgraded Successfully", StatusCodes.Status200OK);
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelSubscription()
        {
            var userId = GetCurrentUserId();
            //var userId = new Guid("00000000-0000-0000-0000-000000000001");

            var command = new CancelSubscriptionCommand(userId);
            var result = await _mediator.Send(command);
            return FromResult(result, "Subscription Canceled Successfully", StatusCodes.Status200OK);
        }
    }
}
