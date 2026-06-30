using AuthService.Features.RefreshToken.Dtos;
using BuildingBlocks.Shared.Results;
using MediatR;

namespace AuthService.Features.RefreshToken
{
    public record RefreshTokenCommand(string refreshToken) : IRequest<Result<RefreshTokenResponse>>;
}
