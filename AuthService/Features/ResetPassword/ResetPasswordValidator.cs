using System.Text.RegularExpressions;
using FluentValidation;

namespace AuthService.Features.ResetPassword
{
    public sealed class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        // >= 6 chars with at least one uppercase letter, one digit and one special character.
        private const string StrongPasswordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{6,}$";

        public ResetPasswordValidator()
        {
            RuleFor(x => x.resetToken)
                .NotEmpty()
                .WithErrorCode("resetToken")
                .WithMessage("Reset token is required.");

            RuleFor(x => x.newPassword)
                .Must(v => !string.IsNullOrWhiteSpace(v) && Regex.IsMatch(v, StrongPasswordPattern))
                .WithErrorCode("newPassword")
                .WithMessage("Password must be at least 6 characters with an uppercase letter, a digit and a special character.");

            RuleFor(x => x.confirmPassword)
                .NotEmpty()
                .WithErrorCode("confirmPassword")
                .WithMessage("Password confirmation is required.");

            RuleFor(x => x.confirmPassword)
                .Equal(x => x.newPassword)
                .When(x => !string.IsNullOrWhiteSpace(x.newPassword) && !string.IsNullOrWhiteSpace(x.confirmPassword))
                .WithErrorCode("AUTH_PASSWORD_MISMATCH")
                .WithMessage("New password and confirmation do not match.");
        }
    }
}
