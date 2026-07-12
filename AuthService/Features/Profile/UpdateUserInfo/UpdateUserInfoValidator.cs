using System.Text.RegularExpressions;
using FluentValidation;

namespace AuthService.Features.Profile.UpdateUserInfo
{
    public sealed class UpdateUserInfoValidator : AbstractValidator<UpdateUserInfoCommand>
    {
        private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        private const string EgyptianPhonePattern = @"^(\+20|0)?1[0125]\d{8}$";

        public UpdateUserInfoValidator()
        {
            RuleFor(x => x.FirstName)
                .Must(v => !string.IsNullOrWhiteSpace(v) && v.Trim().Length is >= 2 and <= 50)
                .WithErrorCode("firstName")
                .WithMessage("First name is required and must be 2-50 characters.");

            RuleFor(x => x.LastName)
                .Must(v => !string.IsNullOrWhiteSpace(v) && v.Trim().Length is >= 2 and <= 50)
                .WithErrorCode("lastName")
                .WithMessage("Last name is required and must be 2-50 characters.");

            RuleFor(x => x.PhoneNumber)
                .Must(v => !string.IsNullOrWhiteSpace(v) && Regex.IsMatch(v, EgyptianPhonePattern))
                .WithErrorCode("phoneNumber")
                .WithMessage("A valid Egyptian phone number is required.");

            RuleFor(x => x.Email)
                .Must(v => !string.IsNullOrWhiteSpace(v) && Regex.IsMatch(v, EmailPattern))
                .WithErrorCode("email")
                .WithMessage("A valid email address is required.");
        }
    }
}
