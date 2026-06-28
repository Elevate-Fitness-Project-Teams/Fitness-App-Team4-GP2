using AuthService.BuildingBlocks.Interfaces;
using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistence.Repositories
{
    public class LoginAttemptRepository : GenericRepository<LoginAttempt>, ILoginAttemptRepository
    {
        public LoginAttemptRepository(AuthDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<LoginAttempt>> GetRecentAttemptsAsync(string email, int count = 10)
        {
            return await _dbSet
            .Where(x => x.Email == email)
            .OrderByDescending(x => x.AttemptedAt)
            .Take(count)
            .ToListAsync();
        }
    }
}
