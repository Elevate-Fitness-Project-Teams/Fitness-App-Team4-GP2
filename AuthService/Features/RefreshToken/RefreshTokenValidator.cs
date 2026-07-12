using FluentValidation;

namespace AuthService.Features.RefreshToken
{
    public sealed class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.refreshToken)
                .NotEmpty()
                .WithErrorCode("refreshToken")
                .WithMessage("Refresh token is required.");
        }
    }
}
