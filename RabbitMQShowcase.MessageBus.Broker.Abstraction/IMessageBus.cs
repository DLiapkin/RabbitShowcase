namespace RabbitMQShowcase.MessageBus.Broker.Abstraction;

public interface IMessageBus
{
    Task PublishAsync<TMessage>(
        TMessage message,
        CancellationToken cancellationToken = default)
        where TMessage : class;

    Task<TResponse> RequestAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken = default,
        TimeSpan? timeout = null)
        where TRequest : class
        where TResponse : class;
}
