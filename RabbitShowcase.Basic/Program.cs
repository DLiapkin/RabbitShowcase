using RabbitMQ.Client;
using System.Text;

namespace RabbitShowcase.Basic.Producer
{
    /// <summary>
    /// Producer
    /// </summary>
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var queueName = "letterbox";
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: true,
                arguments: null);

            const string message = "First letter.";
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: body);
            Console.WriteLine($" [x] Sent {message}");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
