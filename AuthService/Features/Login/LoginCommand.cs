using AuthService.Features.Login.Dtos;
using MediatR;

namespace AuthService.Features.Login
{
    public record LoginCommand (string email, string password) : IRequest<LoginResponse>;
}
