using AuthService.Features.ForgotPassword.Dtos;
using BuildingBlocks.Shared.Results;
using MediatR;

namespace AuthService.Features.ForgotPassword
{
    public  record ForgotPasswordCommand(string Email) : IRequest<Result<ForgotPasswordResponse>>;
}
