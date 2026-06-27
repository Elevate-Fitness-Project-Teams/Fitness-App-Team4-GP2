using AuthService.BuildingBlocks.Helpers;
using AuthService.Features.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public AuthController(IMediator mediatR)
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
    }
}
