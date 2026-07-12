using MassTransit;

namespace AuthService.BuildingBlocks.Interfaces.Events
{
    public class RabbitMqPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public RabbitMqPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        {
            await _publishEndpoint.Publish(message, cancellationToken);
        }
    }
}
