using RabbitMQShowcase.RemoteProcedureCall.Inventory.Models;
using RabbitMQShowcase.RemoteProcedureCall.SDK.Abstraction.Models;

namespace RabbitMQShowcase.RemoteProcedureCall.Inventory.Services;

public class BookStorageService
{
    public Book[] GetAll()
    {
        return [.. new BookFaker().Generate(10)];
    }
}
