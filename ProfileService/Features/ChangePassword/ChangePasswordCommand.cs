using ProfileService.Features.ChangePassword.Dtos;
using BuildingBlocks.Shared.Results;
using MediatR;

namespace ProfileService.Features.ChangePassword
{
    public record ChangePasswordCommand(string CurrentPassword, string NewPassword, string ConfirmPassword) : IRequest<Result<ChangePasswordResponse>>;
}
