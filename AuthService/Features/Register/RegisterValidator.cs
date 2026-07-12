using System.Text.RegularExpressions;
using FluentValidation;

namespace AuthService.Features.Auth.Register
{
    public sealed class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        // >= 6 chars with at least one uppercase letter, one digit and one special character.
        private const string StrongPasswordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{6,}$";
        private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        private const string EgyptianPhonePattern = @"^(\+20|0)?1[0125]\d{8}$";

        public RegisterValidator()
        {
            RuleFor(x => x.FirstName)
                .Must(v => !string.IsNullOrWhiteSpace(v) && v.Trim().Length is >= 2 and <= 50)
                .WithErrorCode("firstName")
                .WithMessage("First name is required and must be 2-50 characters.");

            RuleFor(x => x.LastName)
                .Must(v => !string.IsNullOrWhiteSpace(v) && v.Trim().Length is >= 2 and <= 50)
                .WithErrorCode("lastName")
                .WithMessage("Last name is required and must be 2-50 characters.");

            RuleFor(x => x.Email)
                .Must(v => !string.IsNullOrWhiteSpace(v) && Regex.IsMatch(v, EmailPattern))
                .WithErrorCode("email")
                .WithMessage("A valid email address is required.");

            RuleFor(x => x.Password)
                .Must(v => !string.IsNullOrWhiteSpace(v) && Regex.IsMatch(v, StrongPasswordPattern))
                .WithErrorCode("password")
                .WithMessage("Password must be at least 6 characters with an uppercase letter, a digit and a special character.");

            RuleFor(x => x.PhoneNumber)
                .Must(v => !string.IsNullOrWhiteSpace(v) && Regex.IsMatch(v, EgyptianPhonePattern))
                .WithErrorCode("phoneNumber")
                .WithMessage("A valid Egyptian phone number is required.");
        }
    }
}
