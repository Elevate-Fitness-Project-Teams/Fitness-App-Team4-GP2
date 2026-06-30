using AuthService.Domain.Entities;

namespace AuthService.Infrastructure.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(ApplicationUser user);
        string GenerateRefreshToken();
        string GenerateResetToken(ApplicationUser user);
    }
}
