namespace BuildingBlocks.Contracts.Subscription
{
    public class SubscriptionUpgradedEvent
    {
        public Guid UserId { get; init; }
        public string Plan { get; init; } = default!;
        public bool IsPremium { get; init; }
        public DateTime UpgradedAt { get; init; }
    }
}
