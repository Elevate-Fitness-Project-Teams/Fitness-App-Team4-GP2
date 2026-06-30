using AuthService.BuildingBlocks.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Features.VerifyOtp.Dtos;
using AuthService.Infrastructure.Services.Interfaces;
using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Features.VerifyOtp
{
    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, Result<VerifyOtpResponse>>
    {
        private readonly IOtpRepository _otpRepository;
        private readonly IJwtService _jwtService;
        private readonly IGenericRepository<ApplicationUser> _user;

        public VerifyOtpCommandHandler(IOtpRepository otpRepository, IJwtService jwtService, IGenericRepository<ApplicationUser> user)
        {
            _otpRepository = otpRepository;
            _jwtService = jwtService;
            _user = user;
        }
        public async Task<Result<VerifyOtpResponse>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            var otp = await _otpRepository.GetLatestOtpAsync(request.email);
            if (otp == null || otp.IsUsed) return Error.Validation("AUTH_INVALID_OTP", "Wrong Otp");
            if (otp.ExpiresAt < DateTime.UtcNow) return Error.Validation("AUTH_OTP_EXPIRED", "otp code expired, please request a new one");

            var valid = _otpRepository.Verify(request.otp, otp.Code);

            if (!valid)
                return Error.Validation("AUTH_INVALID_OTP");

            otp.IsUsed = true;
            await _otpRepository.SaveChangesAsync();

            var user = await _user.GetAllAsync(x => x.Email == request.email).FirstOrDefaultAsync();

            if (user is null)
                return Error.NotFound("USER_NOT_FOUND");

            var resetToken = _jwtService.GenerateResetToken(user);


            return new VerifyOtpResponse(resetToken);

        }
    }
}
