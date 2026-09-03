using ClinicProjectApplication.Interfaces;
using ClinicProjectInfrastructure.Common;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;


namespace ClinicProjectInfrastructure.Messaging
{
    // YourApp.Infrastructure/Messaging/RabbitMqPublisher.cs
    public class RabbitMqPublisher : IMessagePublisher, IAsyncDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;

        private RabbitMqPublisher(IConnection connection, IChannel channel)
        {
            _connection = connection;
            _channel = channel;
        }

        public static async Task<RabbitMqPublisher> CreateAsync(RabbitMqOptions options)
        {
            var factory = new ConnectionFactory { HostName = options.HostName, UserName = options.UserName, Password = options.Password };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();
            return new RabbitMqPublisher(connection, channel);
        }

        public async Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default)
        {
            await _channel.QueueDeclareAsync(routingKey, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var props = new BasicProperties { Persistent = true };

            await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: routingKey,
                mandatory: false, basicProperties: props, body: body, cancellationToken: ct);
        }

        public async ValueTask DisposeAsync()
        {
            await _channel.CloseAsync();
            await _connection.CloseAsync();
        }
    }
}
