using System.Text.RegularExpressions;
using FluentValidation;

namespace AuthService.Features.Login
{
    public sealed class LoginValidator : AbstractValidator<LoginCommand>
    {
        private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        public LoginValidator()
        {
            RuleFor(x => x.email)
                .Must(v => !string.IsNullOrWhiteSpace(v) && Regex.IsMatch(v, EmailPattern))
                .WithErrorCode("email")
                .WithMessage("A valid email address is required.");

            RuleFor(x => x.password)
                .NotEmpty()
                .WithErrorCode("password")
                .WithMessage("Password is required.");
        }
    }
}
