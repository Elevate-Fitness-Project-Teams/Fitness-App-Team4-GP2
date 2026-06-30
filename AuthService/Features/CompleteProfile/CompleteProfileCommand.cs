using BuildingBlocks.Shared.Results;
using MediatR;

namespace AuthService.Features.CompleteProfile
{
    public record CompleteProfileCommand : IRequest<Result<bool>>;
}
