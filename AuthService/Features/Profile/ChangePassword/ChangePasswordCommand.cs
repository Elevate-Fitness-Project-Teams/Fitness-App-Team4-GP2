using AuthService.Features.Profile.ChangePassword.Dtos;
using BuildingBlocks.Shared.Results;
using MediatR;

namespace AuthService.Features.Profile.ChangePassword
{
    public record ChangePasswordCommand(string CurrentPassword, string NewPassword, string ConfirmPassword) : IRequest<Result<ChangePasswordResponse>>;
}
