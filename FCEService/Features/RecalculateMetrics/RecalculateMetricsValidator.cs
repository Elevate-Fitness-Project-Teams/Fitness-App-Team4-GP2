using FCEService.Common;
using FCEService.Features.CalculateMetrics;
using FluentValidation;

namespace FCEService.Features.RecalculateMetrics
{
   
        
        public sealed class RecalculateMetricsValidator : AbstractValidator<RecalculateMetricsCommand>
        {
            public RecalculateMetricsValidator()
            {
                RuleFor(c => c.UserId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("A valid UserId is required.");

                RuleFor(c => c.NewWeight!.Value)
                    .InclusiveBetween(40, 200)
                    .WithErrorCode(FceErrors.InvalidWeight.Code)
                    .WithMessage(FceErrors.InvalidWeight.Description)
                    .When(c => c.NewWeight.HasValue);
            }
        }
}
