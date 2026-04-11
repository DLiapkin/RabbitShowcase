namespace RabbitMQShowcase.MessageBus.Broker.Abstraction.Models.Errors;

public class ErrorInfo
{
    public string Code { get; init; } = default;
    public string Message { get; init; } = default;
    public ErrorType Type { get; init; }
    public string[] Details { get; init; } = [];
}
