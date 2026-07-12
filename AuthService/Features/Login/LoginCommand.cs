using AuthService.Features.Login.Dtos;
using BuildingBlocks.Shared.Results;
using MediatR;

namespace AuthService.Features.Login
{
    public record LoginCommand (string email, string password) : IRequest<Result<LoginResponse>>;
}
