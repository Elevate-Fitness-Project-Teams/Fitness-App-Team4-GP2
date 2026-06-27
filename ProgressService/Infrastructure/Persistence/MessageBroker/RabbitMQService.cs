using ProgressService.Shared.Contracts;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class RabbitMQService : IMessagePublisher
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    public RabbitMQService()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;
    }

    public async Task PublishAsync<T>(T message)
    {
        await _channel.ExchangeDeclareAsync(
            exchange: "fitness.events",
            type: ExchangeType.Fanout,
            durable: true);

        var body = Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(message));

        await _channel.BasicPublishAsync(
            exchange: "fitness.events",
            routingKey: typeof(T).Name,
            body: body);
    }
}