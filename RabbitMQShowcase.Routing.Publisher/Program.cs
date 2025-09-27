using RabbitMQ.Client;
using System.Text;

namespace RabbitMQShowcase.Routing.Publisher;

internal class Publisher
{
    static async Task Main(string[] args)
    {
        var counter = 1;
        var message = string.Empty;
        var dictionary = new Dictionary<int, string>()
        {
            { 1, "Info" },
            { 2, "Warning" },
            { 3, "Error" },
        };

        do
        {
            var delay = new Random().Next(500, 1500);
            Thread.Sleep(delay);

            var exchangeName = "direct-logs";
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                autoDelete: true,
                type: ExchangeType.Direct);

            var severity = new Random().Next(1, 3);
            message = $"[{dictionary[severity]}]: Log {counter++}";
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: dictionary[severity],
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
