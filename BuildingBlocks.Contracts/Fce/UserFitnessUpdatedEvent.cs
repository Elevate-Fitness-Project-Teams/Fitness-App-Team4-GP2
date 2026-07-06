namespace BuildingBlocks.Contracts.Fce
{
    /// <summary>
    /// Published by FCEService whenever a user's fitness profile (weight/height) changes.
    /// Consumed by ProfileService to refresh the fitness portion of its cached snapshot.
    /// </summary>
    public class UserFitnessUpdatedEvent
    {
        public Guid UserId { get; init; }
        public double Weight { get; init; }
        public double Height { get; init; }
        public DateTime UpdatedAt { get; init; }
    }
}
