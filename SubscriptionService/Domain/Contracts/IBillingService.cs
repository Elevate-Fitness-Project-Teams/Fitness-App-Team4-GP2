using SubscriptionService.Domain.Entities;
using SubscriptionService.Infrastructure.Services;

namespace SubscriptionService.Domain.Contracts
{
    public interface IBillingService
    {
        Task<BillingResult> AuthorizeAsync(Guid userId , SubscriptionTier planTier , int durationMonths);
    }
}
