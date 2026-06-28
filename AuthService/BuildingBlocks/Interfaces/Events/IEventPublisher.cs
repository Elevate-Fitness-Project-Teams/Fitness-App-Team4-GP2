namespace AuthService.BuildingBlocks.Interfaces.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken = default);
    }
}
