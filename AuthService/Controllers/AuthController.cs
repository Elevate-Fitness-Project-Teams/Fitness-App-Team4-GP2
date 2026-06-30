using AuthService.Features.Auth.Register;
using AuthService.Features.CompleteProfile;
using AuthService.Features.ForgotPassword;
using AuthService.Features.Login;
using AuthService.Features.ResetPassword;
using AuthService.Features.VerifyOtp;
using BuildingBlocks.Shared.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    public class AuthController : ApiControllerBase
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
            return FromResult(result, "User registered successfully.", 201);
        }

        [Authorize]
        [HttpPost("complete-profile")]
        public async Task<IActionResult> CompleteProfile()
        {
            var result = await _mediatR.Send(new CompleteProfileCommand());
            return FromResult(result, "Profile lifecycle initiated.", 200);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediatR.Send(command);
            return FromResult(result, "User logged in successfully.", 200);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
        {
            var result = await _mediatR.Send(command);

            return FromResult(result, "OTP sent successfully.", 200);
        }

        [HttpPost("verify-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyOtp(VerifyOtpCommand command)
        {
            var result = await _mediatR.Send(command);

            return FromResult(result, "OTP verified successfully.", 200);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            var result = await _mediatR.Send(command);

            return FromResult(result, "Password changed successfully.", 200);
        }
    }
}
