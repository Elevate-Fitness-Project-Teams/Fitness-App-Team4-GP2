using AuthService.Features.Login.Dtos;
using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Features.Login
{
    public record LoginCommand ([EmailAddress]string email,string password) : IRequest<LoginResponse>;
}
