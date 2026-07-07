using SubscriptionService.Domain.Entities;

namespace SubscriptionService.Features.UpgradeToPremium
{
    public record UpgradeToPremiumRequest( SubscriptionTier Tier, int DurationMonths);
    
}
