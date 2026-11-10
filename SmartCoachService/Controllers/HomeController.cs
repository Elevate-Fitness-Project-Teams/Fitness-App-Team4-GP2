using BuildingBlocks.Shared.Controllers;
using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCoachService.Domain.Contracts;
using SmartCoachService.Features.GetHomeFeed;

namespace SmartCoachService.Controllers
{
    // Inherits ApiControllerBase route "api/v1/[controller]" -> GET /api/v1/home.
    [Authorize]
    public class HomeController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUser _currentUser;

        public HomeController(IMediator mediator, ICurrentUser currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> GetHomeFeed()
        {
            if (!Guid.TryParse(_currentUser.UserId, out var userId))
            {
                var unauthorized = Result<HomeFeedResponse>.Fail(
                    Error.Unauthorized("AUTH_INVALID_USER", "The authenticated user could not be resolved."));
                return FromResult(unauthorized, string.Empty, StatusCodes.Status401Unauthorized);
            }

            var result = await _mediator.Send(new GetHomeFeedQuery(userId));
            return FromResult(result, "Home feed retrieved successfully.", StatusCodes.Status200OK);
        }
    }
}
