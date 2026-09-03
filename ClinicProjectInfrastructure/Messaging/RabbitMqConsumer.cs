using ClinicProjectApplication.Interfaces;
using ClinicProjectInfrastructure.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClinicProjectInfrastructure.Messaging
{
    // YourApp.Infrastructure/Messaging/RabbitMqConsumer.cs
    public class RabbitMqConsumer<TEvent> : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly RabbitMqOptions _options;
        private readonly string _queueName;
        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMqConsumer(
            IServiceScopeFactory scopeFactory,
            IOptions<RabbitMqOptions> options,
            string queueName)
        {
            _scopeFactory = scopeFactory;
            _options = options.Value;
            _queueName = queueName;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password
            };

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.QueueDeclareAsync(_queueName, durable: true, exclusive: false,
                autoDelete: false, cancellationToken: stoppingToken);

            // process one at a time per consumer instance — tune with BasicQosAsync if needed
            await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                using var scope = _scopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<TEvent>>();

                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var @event = JsonSerializer.Deserialize<TEvent>(json)
                        ?? throw new InvalidOperationException("Deserialized message was null.");

                    await handler.HandleAsync(@event, stoppingToken);
                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
                catch (Exception)
                {
                    // requeue: false → sends to DLX if configured, avoids infinite poison-message loop
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                    // TODO: log the exception (ex) here
                }
            };

            await _channel.BasicConsumeAsync(_queueName, autoAck: false, consumer, stoppingToken);

            // keep running until cancellation
            await Task.Delay(Timeout.Infinite, stoppingToken).ContinueWith(_ => { });
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel is not null) await _channel.CloseAsync(cancellationToken);
            if (_connection is not null) await _connection.CloseAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }
    }
}
