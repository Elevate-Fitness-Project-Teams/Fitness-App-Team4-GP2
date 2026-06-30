using FluentValidation;

namespace ProgressService.Features.LogWeightEntry
{
    public class LogWeightEntryValidator : AbstractValidator<LogWeightEntryCommand>
    {
        public LogWeightEntryValidator()
        {
            RuleFor(x => x.Weight)
            .InclusiveBetween(40, 200)
            .WithMessage("VAL_INVALID_WEIGHT");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("VAL_INVALID_DATE");
        }
    }
}
