using SubscriptionService.Features.GetSubscriptionStatus;

namespace SubscriptionService.Domain.Contracts.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<SubscriptionStatusResponse?> GetSubscriptionStatus(Guid userId);
    }
}
