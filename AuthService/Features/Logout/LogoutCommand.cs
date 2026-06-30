using AuthService.Features.Logout.Dtos;
using BuildingBlocks.Shared.Results;
using MediatR;

namespace AuthService.Features.Logout
{
    public record LogoutCommand : IRequest<Result<LogoutResponse>>;
}
