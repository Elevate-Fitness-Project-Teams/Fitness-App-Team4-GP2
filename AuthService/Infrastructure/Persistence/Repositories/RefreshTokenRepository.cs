using AuthService.BuildingBlocks.Interfaces;
using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AuthDbContext context) : base(context)
        {
        }

        public async Task<IList<RefreshToken>?> GetActiveTokensAsync(Guid userId) =>
            await _dbSet
                .Where(x => x.UserId == userId && x.RevokedAt == null)
                .ToListAsync();


        public async Task<RefreshToken?> GetByTokenAsync(string token) => 
           await _dbSet.FirstOrDefaultAsync(x => x.Token == token);
        
    }
}
