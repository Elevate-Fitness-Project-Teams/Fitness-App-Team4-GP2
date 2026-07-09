using FluentValidation;

namespace FCEService.Features.CalculateMetrics
{
    
        public sealed class CalculateMetricsValidator : AbstractValidator<CalculateMetricsCommand>
        {
            public CalculateMetricsValidator()
            {
                RuleFor(c => c.UserId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("A valid UserId is required.");
            }
        }
    
}
