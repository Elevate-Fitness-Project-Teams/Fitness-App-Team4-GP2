using ProfileService.Features.ChangePassword.Dtos;
using BuildingBlocks.Shared.Results;
using MediatR;
using ProfileService.BuildingBlocks.Interfaces;

namespace ProfileService.Features.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<ChangePasswordResponse>>
    {
        private readonly IAuthClient _authClient;

        public ChangePasswordCommandHandler(IAuthClient authClient)
        {
            _authClient = authClient;
        }

        public async Task<Result<ChangePasswordResponse>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            // AuthService owns the password (Identity verifies the current one and applies policy).
            var result = await _authClient.ChangePasswordAsync(new ChangePasswordRequest(
                request.CurrentPassword,
                request.NewPassword,
                request.ConfirmPassword));

            if (!result.IsSuccess)
                return Result<ChangePasswordResponse>.Fail(result.Errors);

            return new ChangePasswordResponse(true);
        }
    }
}
