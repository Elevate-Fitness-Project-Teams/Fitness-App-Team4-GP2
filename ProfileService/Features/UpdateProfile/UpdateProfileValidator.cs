using System.Text.RegularExpressions;
using FluentValidation;

namespace ProfileService.Features.UpdateProfile
{
    public sealed class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand>
    {
        private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        private const string EgyptianPhonePattern = @"^(\+20|0)?1[0125]\d{8}$";

        public UpdateProfileValidator()
        {
            RuleFor(x => x.Dto)
                .NotNull()
                .WithErrorCode("VAL_REQUIRED_FIELD")
                .WithMessage("Profile data is required.");

            When(x => x.Dto is not null, () =>
            {
                RuleFor(x => x.Dto.FirstName)
                    .Must(v => !string.IsNullOrWhiteSpace(v) && v.Trim().Length is >= 2 and <= 50)
                    .WithErrorCode("firstName")
                    .WithMessage("First name is required and must be 2-50 characters.");

                RuleFor(x => x.Dto.LastName)
                    .Must(v => !string.IsNullOrWhiteSpace(v) && v.Trim().Length is >= 2 and <= 50)
                    .WithErrorCode("lastName")
                    .WithMessage("Last name is required and must be 2-50 characters.");

                RuleFor(x => x.Dto.Email)
                    .Must(v => !string.IsNullOrWhiteSpace(v) && Regex.IsMatch(v, EmailPattern))
                    .WithErrorCode("email")
                    .WithMessage("A valid email address is required.");

                RuleFor(x => x.Dto.PhoneNumber)
                    .Must(v => !string.IsNullOrWhiteSpace(v) && Regex.IsMatch(v, EgyptianPhonePattern))
                    .WithErrorCode("phoneNumber")
                    .WithMessage("A valid Egyptian phone number is required.");
            });
        }
    }
}
