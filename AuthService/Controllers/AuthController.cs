using AuthService.BuildingBlocks.Helpers;
using AuthService.Features.Auth.Register;
using AuthService.Features.CompleteProfile;
using AuthService.Features.Login;
using AuthService.Shared.Responses;
using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MassTransit.ValidationResultExtensions;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly MediatR.IMediator _mediatR;

        public AuthController(MediatR.IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var result = await _mediatR.Send(command);

            return StatusCode(
                201,
                ResponseFactory.Success(
                    result,
                    "User registered successfully.",
                    201));
        }

        [Authorize]
        [HttpPost("complete-profile")]
        public async Task<IActionResult> CompleteProfile()
        {
            var result = await _mediatR.Send(new CompleteProfileCommand());

            return StatusCode(
                201,
                ResponseFactory.Success(
                    result,
                    "Profile lifecycle initiated.",
                    201));

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediatR.Send(command);

            return StatusCode(
                200,
                ResponseFactory.Success(
                    result,
                    "User logged in successfully.",
                    200));
        }

    }
}
