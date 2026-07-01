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
                  .WithErrorCode(FceErrors.RequiredField("UserId").Code)
                  .WithMessage(FceErrors.RequiredField("UserId").Description);


            RuleFor(x => x.Age)
                  .InclusiveBetween(16, 100)
                  .WithErrorCode(FceErrors.InvalidAge.Code)           
                  .WithMessage(FceErrors.InvalidAge.Description);

            RuleFor(x => x.Weight)
                .InclusiveBetween(40, 200)
                .WithErrorCode(FceErrors.InvalidWeight.Code)
                .WithMessage(FceErrors.InvalidWeight.Description);

            RuleFor(x => x.Height)
                .InclusiveBetween(140, 220)
                .WithErrorCode(FceErrors.InvalidHeight.Code)
                .WithMessage(FceErrors.InvalidHeight.Description);

            RuleFor(x => x.Gender)
                .NotEqual(Gender.None)
                .WithErrorCode(FceErrors.InvalidGender.Code)
                .WithMessage(FceErrors.InvalidGender.Description);

            RuleFor(x => x.Goal)
                .NotEqual(FitnessGoal.None)
                .WithErrorCode(FceErrors.InvalidGoal.Code)
                .WithMessage(FceErrors.InvalidGoal.Description);

            RuleFor(x => x.ActivityLevel)
                .NotEqual(ActivityLevel.None)
                .WithErrorCode(FceErrors.InvalidActivity.Code)
                .WithMessage(FceErrors.InvalidActivity.Description);
        }
    }
}
