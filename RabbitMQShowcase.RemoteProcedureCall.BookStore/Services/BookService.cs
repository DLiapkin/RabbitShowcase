using RabbitMQShowcase.RemoteProcedureCall.SDK.Abstraction.Constants;
using RabbitMQShowcase.RemoteProcedureCall.SDK.Abstraction.Models;
using RabbitMQShowcase.RemoteProcedureCall.SDK.Abstraction.Services;

namespace RabbitMQShowcase.RemoteProcedureCall.BookStore.Services;

public class BookService(
    IClient<Book[]> _rpcBooksClient)
{
    public async Task<Book[]> GetAll()
    {
        await _rpcBooksClient.InitializeAsync();

        return await _rpcBooksClient.SendAsync(QueueConstants.InventoryQueueName);
    }
}
