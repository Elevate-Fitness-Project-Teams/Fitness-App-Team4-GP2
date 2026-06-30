using AuthService.Features.Login.Dtos;
using BuildingBlocks.Shared.Results;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Features.Login
{
    public record LoginCommand ([EmailAddress]string email,string password) : IRequest<Result<LoginResponse>>;
}
