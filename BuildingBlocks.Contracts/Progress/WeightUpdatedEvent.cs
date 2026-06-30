namespace BuildingBlocks.Contracts.Progress
{
    public class WeightUpdatedEvent
    {
        public Guid UserId { get; init; }
        public decimal WeightKg { get; init; }
        public DateTime RecordedAt { get; init; }
    }
}
