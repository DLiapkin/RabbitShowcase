using MassTransit;
using RabbitMQShowcase.MessageBus.Broker.Abstraction;

namespace RabbitMQShowcase.MessageBus.Broker;

internal class MessageBus : IMessageBus
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ISendEndpointProvider _sendProvider;
    private readonly IClientFactory _clientFactory;

    public MessageBus(
        IPublishEndpoint publishEndpoint,
        ISendEndpointProvider sendProvider,
        IClientFactory clientFactory)
    {
        _publishEndpoint = publishEndpoint;
        _sendProvider = sendProvider;
        _clientFactory = clientFactory;
    }

    public Task PublishAsync<TMessage>(
        TMessage message, CancellationToken ct = default)
        where TMessage : class
        => _publishEndpoint.Publish(message, ct);

    public async Task<TResponse> RequestAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken ct = default,
        TimeSpan? timeout = null)
        where TRequest : class
        where TResponse : class
    {
        var client = _clientFactory.CreateRequestClient<TRequest>(
            timeout ?? TimeSpan.FromSeconds(30));
        var response = await client.GetResponse<TResponse>(request, ct);

        return response.Message;
    }
}
