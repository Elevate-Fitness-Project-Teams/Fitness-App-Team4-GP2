using AuthService.Domain.Entities;
using System.Security.Claims;

namespace AuthService.Infrastructure.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(ApplicationUser user);
        string GenerateRefreshToken();
        string GenerateResetToken(ApplicationUser user);

        /// <summary>
        /// Validates a password-reset token (signature, issuer/audience, lifetime and purpose).
        /// Returns the principal on success, or null if the token is invalid or expired.
        /// </summary>
        ClaimsPrincipal? ValidateResetToken(string token);
    }
}
