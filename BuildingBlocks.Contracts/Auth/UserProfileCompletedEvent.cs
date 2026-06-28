namespace BuildingBlocks.Contracts.Auth
{
    public class UserProfileCompletedEvent
    {
        public Guid UserId { get; init; }
        public string Email { get; init; } = default!;
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string PhoneNumber { get; init; } = default!;
        public bool IsPremium { get; init; }
        public DateTime CompletedAt { get; init; }
    }
}
