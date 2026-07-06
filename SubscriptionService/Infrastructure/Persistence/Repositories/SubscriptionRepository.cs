using Microsoft.EntityFrameworkCore;
using SubscriptionService.Domain.Contracts.Repositories;
using SubscriptionService.Domain.Entities;
using SubscriptionService.Features.GetSubscriptionStatus;

namespace SubscriptionService.Infrastructure.Persistence.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly SubscriptionDbContext _subscriptionDbContext;
        private readonly DbSet<UserSubscription> _dbSet;

        public SubscriptionRepository(SubscriptionDbContext subscriptionDbContext )
        {
            _subscriptionDbContext = subscriptionDbContext;
            _dbSet = _subscriptionDbContext.Set<UserSubscription>();
        }

        public async Task<SubscriptionStatusResponse?> GetSubscriptionStatus(Guid userId)
        {
            return await _dbSet.Where(x=>x.UserId == userId)
                .Select(x => new SubscriptionStatusResponse
                (
                    x.Tier,
                    x.Status,
                    x.ExpiresAt
                ))
                .FirstOrDefaultAsync();
        }
    }
}
