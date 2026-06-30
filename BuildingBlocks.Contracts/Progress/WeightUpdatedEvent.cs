namespace BuildingBlocks.Contracts.Progress
{
    public class WeightUpdatedEvent
    {
        public Guid UserId { get; init; }
        public double WeightKg { get; init; }
        public DateTime RecordedAt { get; init; }
    }
}
