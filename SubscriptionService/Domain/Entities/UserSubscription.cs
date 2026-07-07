using System;
namespace SubscriptionService.Domain.Entities
{
    public class UserSubscription
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public SubscriptionTier Tier { get; set; } = SubscriptionTier.Free;
        public string Status { get; set; } = "Active";
        public DateTime StartsAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}