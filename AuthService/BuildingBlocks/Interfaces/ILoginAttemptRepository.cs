using AuthService.Domain.Entities;

namespace AuthService.BuildingBlocks.Interfaces
{
    public interface ILoginAttemptRepository : IGenericRepository<LoginAttempt>
    {
        Task<IEnumerable<LoginAttempt>> GetRecentAttemptsAsync(string email, int count = 10);
    }
}
