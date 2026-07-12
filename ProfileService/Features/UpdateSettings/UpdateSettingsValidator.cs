using FluentValidation;

namespace ProfileService.Features.UpdateSettings
{
    public sealed class UpdateSettingsValidator : AbstractValidator<UpdateSettingsCommand>
    {
        public UpdateSettingsValidator()
        {
            // True PATCH: at least one settings section must be supplied.
            RuleFor(x => x.Request)
                .Must(r => r is not null &&
                           (r.Preferences is not null || r.Notifications is not null || r.Privacy is not null))
                .WithErrorCode("VAL_REQUIRED_FIELD")
                .WithMessage("No settings fields were supplied.");
        }
    }
}
