using AuthService.BuildingBlocks.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Features.ForgotPassword.Dtos;
using AuthService.Infrastructure.Services.Interfaces;
using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Features.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<ForgotPasswordResponse>>
    {
        private const int OtpExpirySeconds = 600;      // 10 minutes
        private const int ResendCooldownSeconds = 30;

        private readonly IGenericRepository<ApplicationUser> _userRepo;
        private readonly IOtpRepository _otpRepository;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(
            IGenericRepository<ApplicationUser> userRepo,
            IOtpRepository otpRepository,
            IEmailService emailService)
        {
            _userRepo = userRepo;
            _otpRepository = otpRepository;
            _emailService = emailService;
        }
        public async Task<Result<ForgotPasswordResponse>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetAllAsync(x => x.Email ==  request.Email).FirstOrDefaultAsync();

            if (user is null)
                return Error.NotFound("USER_NOT_FOUND");

            var now = DateTime.UtcNow;

            var recentOtp = await _otpRepository.GetLatestOtpAsync(request.Email);

            // Resend is allowed only after the 30-second cooldown since the last code was created.
            if (recentOtp != null && recentOtp.CreatedAt > now.AddSeconds(-ResendCooldownSeconds))
                return Error.TooManyRequestsException("RATE_OTP_RESEND_TOO_SOON");

            var otp = _otpRepository.Generate();
            var hashedOtp = _otpRepository.Hash(otp);

            await _otpRepository.AddAsync(new OtpCode
            {
                Email = request.Email,
                Code = hashedOtp,
                CreatedAt = now,
                ExpiresAt = now.AddSeconds(OtpExpirySeconds),
                IsUsed = false
            });

            await _otpRepository.SaveChangesAsync();

            await _emailService.SendAsync(
                request.Email,
                "Your Elevate password reset code",
                $"Your verification code is {otp}. It expires in 10 minutes. " +
                "If you did not request this, you can ignore this email.",
                cancellationToken);

            return new ForgotPasswordResponse(
                request.Email,
                OtpExpirySeconds,
                ResendCooldownSeconds);

        }
    }
}
