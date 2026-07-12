using System.Text.RegularExpressions;
using FluentValidation;

namespace AuthService.Features.VerifyOtp
{
    public sealed class VerifyOtpValidator : AbstractValidator<VerifyOtpCommand>
    {
        private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        private const string SixDigitPattern = @"^\d{6}$";

        public VerifyOtpValidator()
        {
            RuleFor(x => x.email)
                .Must(v => !string.IsNullOrWhiteSpace(v) && Regex.IsMatch(v, EmailPattern))
                .WithErrorCode("email")
                .WithMessage("A valid email address is required.");

            RuleFor(x => x.otp)
                .Must(v => !string.IsNullOrWhiteSpace(v) && Regex.IsMatch(v, SixDigitPattern))
                .WithErrorCode("otp")
                .WithMessage("OTP must be a 6-digit code.");
        }
    }
}
