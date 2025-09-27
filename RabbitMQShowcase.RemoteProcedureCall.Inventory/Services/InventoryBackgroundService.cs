using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQShowcase.RemoteProcedureCall.SDK.Abstraction.Constants;
using RabbitMQShowcase.RemoteProcedureCall.SDK.Abstraction.Models;
using System.Text;
using System.Text.Json;

namespace RabbitMQShowcase.RemoteProcedureCall.Inventory.Services;

public class InventoryBackgroundService : BackgroundService
{
    private readonly ILogger<InventoryBackgroundService> _logger;
    private readonly BookStorageService _bookStorageService;
    private IConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IChannel _channel;

    public InventoryBackgroundService(
        ILogger<InventoryBackgroundService> logger,
        BookStorageService bookStorageService)
    {
        _logger = logger;
        _bookStorageService = bookStorageService;
        _connectionFactory = new ConnectionFactory() { HostName = "localhost" };
    }

    private async Task Initialize()
    {
        Console.WriteLine($" [.] Inventory background service's started");
        _connection = await _connectionFactory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        await _channel.QueueDeclareAsync(
            queue: QueueConstants.InventoryQueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        await Initialize();

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (sender, ea) =>
        {
            var cons = (AsyncEventingBasicConsumer)sender;
            var channel = cons.Channel;

            var props = ea.BasicProperties;
            var replyProps = new BasicProperties
            {
                CorrelationId = props.CorrelationId
            };

            var response = Array.Empty<Book>();

            try
            {
                //var filterBytes = ea.Body.ToArray();
                //var message = Encoding.UTF8.GetString(filterBytes);

                Console.WriteLine($" [.] Sending reply to {props.CorrelationId}");
                response = _bookStorageService.GetAll();
            }
            catch (Exception e)
            {
                Console.WriteLine($" [.] {e.Message}");
            }
            finally
            {
                var responseJson = JsonSerializer.Serialize(response);
                var responseBytes = Encoding.UTF8.GetBytes(responseJson);

                await channel.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: props.ReplyTo!,
                    mandatory: true,
                    basicProperties: replyProps,
                    body: responseBytes);

                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            }
        };

        await _channel.BasicConsumeAsync(
            queue: QueueConstants.InventoryQueueName,
            autoAck: false,
            consumer: consumer);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
        await base.StopAsync(cancellationToken);
    }
}