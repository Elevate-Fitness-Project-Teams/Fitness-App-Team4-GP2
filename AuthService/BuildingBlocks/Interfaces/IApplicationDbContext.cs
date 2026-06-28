using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.BuildingBlocks.Interfaces
{
    /// <summary>
    /// Abstraction over the application's EF Core context so feature handlers
    /// can depend on persistence without referencing the concrete DbContext.
    /// </summary>
    public interface IApplicationDbContext
    {
        DbSet<ApplicationUser> Users { get; }
        DbSet<OtpCode> OtpCodes { get; }
        DbSet<RefreshToken> RefreshTokens { get; }
        DbSet<LoginAttempt> LoginAttempts { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
