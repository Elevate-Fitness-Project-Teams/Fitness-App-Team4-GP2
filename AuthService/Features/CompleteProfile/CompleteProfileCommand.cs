using MediatR;

namespace AuthService.Features.CompleteProfile
{
    public record CompleteProfileCommand : IRequest<bool>;
}
