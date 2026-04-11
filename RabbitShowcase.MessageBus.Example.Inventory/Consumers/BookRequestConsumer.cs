using MassTransit;
using RabbitMQShowcase.MessageBus.Broker;
using RabbitShowcase.MessageBus.Example.Inventory.Services;
using RabbitShowcase.MessageBus.Example.SDK.Abstraction.Model.Broker.Request;
using RabbitShowcase.MessageBus.Example.SDK.Abstraction.Model.Broker.Response;

namespace RabbitShowcase.MessageBus.Example.Inventory.Consumers;

public class BookRequestConsumer(
    ILogger<BookRequestConsumer> _logger,
    BookStorageService _service) : SafeConsumer<BookRequest, BookResponse>(_logger)
{
    protected override async Task<BookResponse> Handle(ConsumeContext<BookRequest> context, CancellationToken cancellationToken)
    {
        var request = context.Message;
        if (request != null)
        {
            var models = _service.GetAll();
            return await Task.FromResult(new BookResponse(models));
        }
        else
        {
            return await Task.FromResult(new BookResponse([]));
        }
    }
}
