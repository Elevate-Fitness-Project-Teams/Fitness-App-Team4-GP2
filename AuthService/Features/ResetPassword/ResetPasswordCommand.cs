using AuthService.Features.ResetPassword.Dtos;
using BuildingBlocks.Shared.Results;
using MediatR;

namespace AuthService.Features.ResetPassword
{
    public record ResetPasswordCommand(string resetToken, string newPassword, string confirmPassword ) : IRequest<Result<ResetPasswordResponse>>;
}
