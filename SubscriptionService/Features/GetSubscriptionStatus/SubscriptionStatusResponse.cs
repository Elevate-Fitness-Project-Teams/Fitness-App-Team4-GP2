namespace SubscriptionService.Features.GetSubscriptionStatus
{
    public record SubscriptionStatusResponse(string Tier , string Status , DateTime? ExpiresAt , DateTime? UpdatedAt);
    
}