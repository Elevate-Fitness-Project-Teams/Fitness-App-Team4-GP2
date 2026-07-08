using BuildingBlocks.Shared.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Features.UpdateProfile;
using ProfileService.Features.UpdateProfile.Dtos;
using ProfileService.Features.UploadProfilePicture;
using ProfileService.Features.ViewProfile;
using System.Security.Claims;

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

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request)
        {
            var result = await _mediator.Send(new UpdateProfileCommand(request));

            return FromResult(result, "Profile updated successfully.", 200);
        }

        [Authorize]
        [HttpPost("picture")]
        public async Task<IActionResult> UploadPicture([FromForm] IFormFile profilePicture)
        {

            var result = await _mediator.Send(new UploadProfilePictureCommand(profilePicture));

            return FromResult(result, "Profile picture updated successfully.", 200);
        }
    }
}
