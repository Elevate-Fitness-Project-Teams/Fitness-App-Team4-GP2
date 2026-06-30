using AuthService.BuildingBlocks.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Features.ResetPassword.Dtos;
using AuthService.Infrastructure.Services.Interfaces;
using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthService.Features.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<ResetPasswordResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IGenericRepository<Domain.Entities.RefreshToken> _refreshTokens;

        public ResetPasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService,
            IGenericRepository<Domain.Entities.RefreshToken> refreshTokens)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _refreshTokens = refreshTokens;
        }

        public async Task<Result<ResetPasswordResponse>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.newPassword != request.confirmPassword)
                return Error.Validation("AUTH_PASSWORD_MISMATCH", "New password and confirmation do not match.");

            var principal = _jwtService.ValidateResetToken(request.resetToken);
            if (principal is null)
                return Error.Validation("AUTH_RESET_TOKEN_INVALID", "The reset token is invalid or has expired.");

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Error.Validation("AUTH_RESET_TOKEN_INVALID", "The reset token is invalid or has expired.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Error.Validation("AUTH_RESET_TOKEN_INVALID", "The reset token is invalid or has expired.");

            // Single-use: the token carries the security stamp it was issued with. A previous
            // successful reset rotates the stamp, so a used/superseded token no longer matches.
            var tokenStamp = principal.FindFirst("stamp")?.Value ?? string.Empty;
            if (tokenStamp != (user.SecurityStamp ?? string.Empty))
                return Error.Validation("AUTH_RESET_TOKEN_INVALID", "The reset token has already been used.");


            foreach (var validator in _userManager.PasswordValidators)
            {
                var validation = await validator.ValidateAsync(_userManager, user, request.newPassword);
                if (!validation.Succeeded)
                    return Error.Validation("AUTH_PASSWORD_POLICY",
                        string.Join(", ", validation.Errors.Select(e => e.Description)));
            }

            // Set the new hash and rotate the security stamp (persists both, invalidating the token).
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.newPassword);
            await _userManager.UpdateSecurityStampAsync(user);

            // Revoke all active refresh tokens for the user.
            var activeTokens = await _refreshTokens
                .GetAllAsync(t => t.UserId == user.Id && t.RevokedAt == null)
                .ToListAsync(cancellationToken);

            foreach (var token in activeTokens)
                token.RevokedAt = DateTime.UtcNow;

            await _refreshTokens.SaveChangesAsync();

            return new ResetPasswordResponse(true);
        }
    }
}
