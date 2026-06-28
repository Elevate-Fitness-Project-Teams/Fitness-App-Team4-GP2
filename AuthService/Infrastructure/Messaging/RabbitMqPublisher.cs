using AuthService.BuildingBlocks.Interfaces.Events;
using MassTransit;

namespace AuthService.Infrastructure.Messaging
{
    public class RabbitMqPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public RabbitMqPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(
            T message,
            CancellationToken cancellationToken = default)
        {
            await _publishEndpoint.Publish(message, cancellationToken);
        }
    }
}
