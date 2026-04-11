namespace RabbitMQShowcase.MessageBus.Broker.Abstraction.Models.Errors;

public enum ErrorType
{
    Validation,
    Conflict,
    NotFound,
    Unexpected
}
