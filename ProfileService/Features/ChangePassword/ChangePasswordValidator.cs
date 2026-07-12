using System.Text.RegularExpressions;
using FluentValidation;

namespace ProfileService.Features.ChangePassword
{
    public sealed class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        // >= 6 chars with at least one uppercase letter, one digit and one special character.
        private const string StrongPasswordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{6,}$";

        public ChangePasswordValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithErrorCode("currentPassword")
                .WithMessage("Current password is required.");

            RuleFor(x => x.NewPassword)
                .Must(v => !string.IsNullOrWhiteSpace(v) && Regex.IsMatch(v, StrongPasswordPattern))
                .WithErrorCode("newPassword")
                .WithMessage("Password must be at least 6 characters with an uppercase letter, a digit and a special character.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithErrorCode("confirmPassword")
                .WithMessage("Password confirmation is required.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword) && !string.IsNullOrWhiteSpace(x.ConfirmPassword))
                .WithErrorCode("AUTH_PASSWORD_MISMATCH")
                .WithMessage("Passwords do not match.");
        }
    }
}
