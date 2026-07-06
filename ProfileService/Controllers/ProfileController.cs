using BuildingBlocks.Shared.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Features.ViewProfile;

namespace ProfileService.Controllers
{
    public class ProfileController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _mediator.Send(new ViewProfileQuery());

            return FromResult(result, "Profile retrieved successfully.", 200);
        }
    }
}
