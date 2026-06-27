using FluentValidation;

namespace ProgressService.Features.WorkoutCompletion
{
    public class WorkOutCompletionValidator : AbstractValidator<WorkoutCompletionCommand>
    {
        public WorkOutCompletionValidator()
        {
            RuleFor(x => x.WorkoutId).NotEmpty().WithMessage("WorkoutId is required.");
            RuleFor(x => x.SessionId).NotEmpty().WithMessage("SessionId is required.");
            RuleFor(x => x.DurationInMinutes).GreaterThan(0).WithMessage("DurationInMinutes must be greater than 0.");
            RuleFor(x => x.CaloriesBurned).GreaterThan(0).WithMessage("CaloriesBurned must be greater than 0.");
            RuleFor(x => x.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
            RuleFor(x => x.Notes).MaximumLength(500).WithMessage("Notes must not exceed 500 characters.");
        }
    }
}
