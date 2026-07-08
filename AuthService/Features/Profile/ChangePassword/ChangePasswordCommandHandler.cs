using AuthService.BuildingBlocks.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Features.Profile.ChangePassword.Dtos;
using AuthService.Infrastructure.Persistence.Repositories;
using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Features.Profile.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<ChangePasswordResponse>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRefreshTokenRepository _tokenRepository;

        public ChangePasswordCommandHandler(ICurrentUser currentUser, UserManager<ApplicationUser> userManager,  IRefreshTokenRepository tokenRepository)
        {
            _currentUser = currentUser;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }
        public async Task<Result<ChangePasswordResponse>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if(!Guid.TryParse(_currentUser.UserId, out var userId))
                return Error.Unauthorized("AUTH_TOKEN_INVALID", "Authentication token is missing or invalid.");

            //redundant
            //if (request.NewPassword != request.ConfirmPassword)
            //    return Error.Validation("AUTH_PASSWORD_MISMATCH", "New password and confirmation do not match.");

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return Error.Unauthorized("AUTH_USER_NOT_FOUND");

            // UserName is ignored in the DB model, so it loads back null and trips Identity's
            // user validator during the update inside ChangePasswordAsync. Restore it
            // (username == email in this system) so the change validates.
            user.UserName = user.Email;

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
                return Error.Unauthorized("AUTH_INVALID_CREDENTIALS.");


            var activeTokens = await _tokenRepository.GetActiveTokensAsync(userId);

            if (activeTokens is not null)
                foreach (var token in activeTokens)
                    token.RevokedAt = DateTime.UtcNow;

            await _tokenRepository.SaveChangesAsync();

            return new ChangePasswordResponse(true);
        }
    }
}
