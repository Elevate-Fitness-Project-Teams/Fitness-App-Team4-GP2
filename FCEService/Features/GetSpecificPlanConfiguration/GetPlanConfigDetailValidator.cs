using FluentValidation;

namespace FCEService.Features.GetSpecificPlanConfiguration
{
    public sealed class GetPlanConfigDetailValidator : AbstractValidator<GetPlanConfigDetailQuery>
    {
        public GetPlanConfigDetailValidator()
        {
            RuleFor(q => q.PlanId)
                .NotEmpty()
                .WithMessage("A valid PlanId is required.");
        }
    }
}
