using Bogus;
using RabbitShowcase.MessageBus.Example.SDK.Abstraction.Model;

namespace RabbitShowcase.MessageBus.Example.Inventory.Models;

public class BookFaker : Faker<Book>
{
    public BookFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Number(1, 1000));
        RuleFor(x => x.Name, f => f.Music.Random.Word());
        RuleFor(x => x.Description, f => f.Lorem.Sentence(20));
        RuleFor(x => x.Author, f => f.Person.FullName);
    }
}
