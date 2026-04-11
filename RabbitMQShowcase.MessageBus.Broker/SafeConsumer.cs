using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitMQShowcase.MessageBus.Broker.Abstraction.Models;
using RabbitMQShowcase.MessageBus.Broker.Abstraction.Models.Errors;

namespace RabbitMQShowcase.MessageBus.Broker;

public abstract class SafeConsumer<TRequest, TResponse> : IConsumer<TRequest>
    where TRequest : class
    where TResponse : class
{
    private readonly ILogger _logger;

    protected SafeConsumer(ILogger logger)
    {
        _logger = logger;
    }

    protected abstract Task<TResponse> Handle(
        ConsumeContext<TRequest> context,
        CancellationToken cancellationToken);

    public async Task Consume(ConsumeContext<TRequest> context)
    {
        try
        {
            var data = await Handle(context, context.CancellationToken);
            await context.RespondAsync(Result<TResponse>.Ok(data));
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "DomainException errors processing {MessageType}", typeof(TRequest).Name);

            await context.RespondAsync(
                Result<TResponse>.Fail(
                    code: "409",
                    message: "Unexpected error",
                    type: ErrorType.Conflict,
                    details: [.. ex.Details]));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error processing {MessageType}", typeof(TRequest).Name);

            await context.RespondAsync(
                Result<TResponse>.Fail(
                    code: "500",
                    message: "Unexpected error",
                    type: ErrorType.Unexpected,
                    details: [ex.Message]));
        }
    }
}
