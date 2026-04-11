using RabbitShowcase.MessageBus.Example.Inventory.Models;
using RabbitShowcase.MessageBus.Example.SDK.Abstraction.Model;

namespace RabbitShowcase.MessageBus.Example.Inventory.Services;

public class BookStorageService
{
    public Book[] GetAll()
    {
        return [.. new BookFaker().Generate(10)];
    }
}
