using BuildingBlocks.Shared.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Features.ChangePassword;
using ProfileService.Features.UpdateProfile;
using ProfileService.Features.UpdateProfile.Dtos;
using ProfileService.Features.UpdateSettings;
using ProfileService.Features.UpdateSettings.Dtos;
using ProfileService.Features.UploadProfilePicture;
using ProfileService.Features.ViewProfile;
using ProfileService.Features.ViewSettings;

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
        [Consumes("multipart/form-data")]
        // Allow bodies beyond Kestrel's 30MB default so oversized files reach our own
        // validation and get the spec'd 400 VAL_FILE_TOO_LARGE instead of a connection reset.
        [RequestSizeLimit(50 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 50 * 1024 * 1024)]
        public async Task<IActionResult> UploadPicture(IFormFile profilePicture)
        {

            var result = await _mediator.Send(new UploadProfilePictureCommand(profilePicture));

            return FromResult(result, "Profile picture updated successfully.", 200);
        }

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            var result = await _mediator.Send(command);

            return FromResult(result, "Password changed successfully.", StatusCodes.Status200OK);
        }

        [Authorize]
        [HttpGet("/api/v1/settings")]
        public async Task<IActionResult> GetSettings()
        {
            var result = await _mediator.Send(new ViewSettingsQuery());

            return FromResult(result, "Settings retrieved successfully.", 200);
        }

        [Authorize]
        [HttpPut("/api/v1/settings")]
        public async Task<IActionResult> UpdateSettings(UpdateSettingsRequest request)
        {
            var result = await _mediator.Send(new UpdateSettingsCommand(request));

            return FromResult(result, "Settings Updated successfully.", 200);
        }
    }
}
