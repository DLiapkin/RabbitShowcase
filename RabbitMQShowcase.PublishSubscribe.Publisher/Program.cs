using RabbitMQ.Client;
using System.Text;

namespace RabbitMQShowcase.PublishSubscribe.Publisher;

internal class Publisher
{
    static async Task Main(string[] args)
    {
        var counter = 1;
        do
        {
            var delay = new Random().Next(500, 1500);
            Thread.Sleep(delay);

            var exchangeName = "logs";
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Fanout);

            string message = $"Log [{counter++}]";
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: string.Empty,
                body: body);

            Console.WriteLine($" [x] Sent: {message}");

            if (counter > 30)
            {
                break;
            }
        }
        while (true);
    }
}
