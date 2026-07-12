using System.Text.RegularExpressions;
using FluentValidation;

namespace AuthService.Features.ForgotPassword
{
    public sealed class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
    {
        private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email)
                .Must(v => !string.IsNullOrWhiteSpace(v) && Regex.IsMatch(v, EmailPattern))
                .WithErrorCode("email")
                .WithMessage("A valid email address is required.");
        }
    }
}
