using RabbitMQShowcase.MessageBus.Broker.Abstraction.Models.Errors;

namespace RabbitMQShowcase.MessageBus.Broker.Abstraction.Models;

public class Result<T>
{
    public bool IsSuccess { get; init; }
    public T? Data { get; init; }
    public ErrorInfo? Error { get; init; }

    public static Result<T> Ok(T Data) => new()
    {
        IsSuccess = true,
        Data = Data
    };

    public static Result<T> Fail(
        string code,
        string message,
        ErrorType type = ErrorType.Unexpected,
        string[] details = null!) => new()
        {
            IsSuccess = false,
            Error = new ErrorInfo
            {
                Code = code,
                Message = message,
                Type = type,
                Details = details ?? []
            }
        };
}
