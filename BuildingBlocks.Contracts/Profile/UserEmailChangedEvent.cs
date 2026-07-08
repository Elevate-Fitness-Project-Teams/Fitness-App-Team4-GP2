
namespace BuildingBlocks.Contracts.Profile
{
    public class UserEmailChangedEvent
    {
        public Guid UserId { get; init; }
        public string OldEmail { get; init; } = default!;
        public string NewEmail { get; init; } = default!;
    }
}
