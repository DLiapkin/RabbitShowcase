using RabbitMQShowcase.MessageBus.Broker.Abstraction;
using RabbitMQShowcase.MessageBus.Broker.Abstraction.Models;
using RabbitShowcase.MessageBus.Example.SDK.Abstraction.Model;
using RabbitShowcase.MessageBus.Example.SDK.Abstraction.Model.Broker.Request;
using RabbitShowcase.MessageBus.Example.SDK.Abstraction.Model.Broker.Response;

namespace RabbitShowcase.MessageBus.Example.BookStore.Services;

public class BookService(
    IMessageBus _messageBus)
{
    public async Task<Book[]> GetAll()
    {
        var response = await _messageBus.RequestAsync<BookRequest, Result<BookResponse>>(new([]));

        if (response != null && response.IsSuccess && response.Data != null)
        {
            return response.Data.Books;
        }

        return [];
    }
}
