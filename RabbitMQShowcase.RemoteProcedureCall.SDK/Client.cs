using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQShowcase.RemoteProcedureCall.SDK.Abstraction.Services;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace RabbitMQShowcase.RemoteProcedureCall.SDK;

public class Client<TResult> : IClient<TResult>
{
    private IConnectionFactory _connectionFactory;
    private IConnection? _connection;
    private IChannel? _channel;
    private string? _replyQueueName;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<TResult>> _callbackMapper = new();

    public Client()
    {
        _connectionFactory = new ConnectionFactory() { HostName = "localhost" };
    }

    public async Task InitializeAsync()
    {
        _connection = await _connectionFactory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        var queueDeclareResult = await _channel.QueueDeclareAsync();
        _replyQueueName = queueDeclareResult.QueueName;
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += (model, ea) =>
        {
            string? correlationId = ea.BasicProperties.CorrelationId;

            if (false == string.IsNullOrEmpty(correlationId))
            {
                if (_callbackMapper.TryRemove(correlationId, out var tcs))
                {
                    var body = ea.Body.ToArray();
                    var response = Encoding.UTF8.GetString(body);
                    var result = JsonSerializer.Deserialize<TResult>(response);
                    tcs.TrySetResult(result);
                }
            }

            return Task.CompletedTask;
        };

        await _channel.BasicConsumeAsync(_replyQueueName, true, consumer);
    }

    public async Task<TResult> SendAsync(string queueName, object? args = null, CancellationToken cancellationToken = default)
    {
        await InitializeAsync();

        if (_channel is null)
        {
            throw new InvalidOperationException();
        }

        string correlationId = Guid.NewGuid().ToString();
        var props = new BasicProperties
        {
            CorrelationId = correlationId,
            ReplyTo = _replyQueueName
        };

        var tcs = new TaskCompletionSource<TResult>(
                TaskCreationOptions.RunContinuationsAsynchronously);
        _callbackMapper.TryAdd(correlationId, tcs);

        var argsJson = string.Empty;
        if (args != null)
        {
            argsJson = JsonSerializer.Serialize(args);
        }

        var messageBytes = Encoding.UTF8.GetBytes(argsJson);
        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            mandatory: true,
            basicProperties: props,
            body: messageBytes,
            cancellationToken: cancellationToken);

        using CancellationTokenRegistration ctr =
            cancellationToken.Register(() =>
            {
                _callbackMapper.TryRemove(correlationId, out _);
                tcs.SetCanceled();
            });

        return await tcs.Task;
    }
}
