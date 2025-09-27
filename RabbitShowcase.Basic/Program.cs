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
            var counter = 1;
            do
            {
                var queueName = "letterbox";
                var factory = new ConnectionFactory { HostName = "localhost" };
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = $"Letter [{counter++}]";
                var body = Encoding.UTF8.GetBytes(message);

                await channel.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: queueName,
                    body: body);

                Console.WriteLine($" [x] Sent: {message}");

                if (!string.IsNullOrEmpty(Console.ReadLine()))
                {
                    break;
                }
            }
            while (true);
        }
    }
}
