using FCEService.Common;
using FCEService.Domain.Enums;
using FluentValidation;


namespace FCEService.Features.SaveFitnessStats
{
    public sealed class SubmitFitnessStatsValidator : AbstractValidator<SubmitFitnessStatsCommand>
    {
    

        public SubmitFitnessStatsValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(FceErrors.RequiredField("UserId").Description);

           
            RuleFor(x => x.Age)
                .InclusiveBetween(16, 100)
                .WithMessage(FceErrors.InvalidAge.Description);

            RuleFor(x => x.Weight)
                .InclusiveBetween(40, 200)
                .WithMessage(FceErrors.InvalidWeight.Description);

            RuleFor(x => x.Height)
                .InclusiveBetween(140, 220)
                .WithMessage(FceErrors.InvalidHeight.Description);

         
            RuleFor(x => x.Gender)
              .NotEqual(Gender.None)
                .WithMessage(FceErrors.InvalidGender.Description);

            RuleFor(x => x.Goal)
                .NotEqual(FitnessGoal.None)
                .WithMessage(FceErrors.InvalidGoal.Description);

            RuleFor(x => x.ActivityLevel)
                .NotEqual(ActivityLevel.None)
                .WithMessage(FceErrors.InvalidActivity.Description);

        }
    }
}
