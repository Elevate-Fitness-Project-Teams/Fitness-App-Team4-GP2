using AuthService.Domain.Entities;

namespace AuthService.BuildingBlocks.Interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        public Task<RefreshToken?> GetByTokenAsync(string token);
        public Task<IList<RefreshToken>?> GetActiveTokensAsync(Guid userId);
    }
}
