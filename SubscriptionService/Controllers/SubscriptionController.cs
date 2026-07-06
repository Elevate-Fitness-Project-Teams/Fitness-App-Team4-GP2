using BuildingBlocks.Shared.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SubscriptionService.Controllers
{
    //[Authorize]
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
    }
}
