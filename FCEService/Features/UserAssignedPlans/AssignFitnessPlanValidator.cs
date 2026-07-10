namespace FCEService.Features.UserAssignedPlans
{
    using FluentValidation;

    namespace FCEService.Features.AssignPlan
    {
        public sealed class AssignFitnessPlanValidator : AbstractValidator<AssignFitnessPlanCommand>
        {
            public AssignFitnessPlanValidator()
            {
                RuleFor(c => c.UserId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("A valid UserId is required.");
            }
        }
    }
}
