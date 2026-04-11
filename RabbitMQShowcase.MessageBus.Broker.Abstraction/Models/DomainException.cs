namespace RabbitMQShowcase.MessageBus.Broker.Abstraction.Models;

public class DomainException : Exception
{
    public string Message { get; init; }
    public string[] Details { get; init; }

    public DomainException(string message, string[] details) : base(message)
    {
        Message = message;
        Details = details;
    }
}
