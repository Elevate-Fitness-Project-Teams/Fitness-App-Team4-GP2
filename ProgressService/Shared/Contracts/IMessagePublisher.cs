namespace ProgressService.Shared.Contracts
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(T message);
    }
}
