namespace RabbitMQShowcase.RemoteProcedureCall.SDK.Abstraction.Services;

public interface IClient<TResult>
{
    public Task InitializeAsync();
    public Task<TResult> SendAsync(string queueName, object? args = null, CancellationToken cancellationToken = default);
}
