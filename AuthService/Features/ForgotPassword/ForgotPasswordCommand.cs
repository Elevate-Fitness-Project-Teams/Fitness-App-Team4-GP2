using AuthService.Features.ForgotPassword.Dtos;
using BuildingBlocks.Shared.Results;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Features.ForgotPassword
{
    public  record ForgotPasswordCommand([EmailAddress]string Email) : IRequest<Result<ForgotPasswordResponse>>;
}
