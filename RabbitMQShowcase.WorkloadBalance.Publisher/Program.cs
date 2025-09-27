using RabbitMQ.Client;
using System.Text;

namespace RabbitMQShowcase.WorkloadBalance.Publisher
{
    internal class Publisher
    {
        static async Task Main(string[] args)
        {
            var counter = 1;
            do
            {
                var delay = new Random().Next(500, 1500);
                Thread.Sleep(delay);

                var queueName = "workload";
                var factory = new ConnectionFactory { HostName = "localhost" };
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: true,
                    arguments: null);

                string message = $"Task [{counter++}]";
                var body = Encoding.UTF8.GetBytes(message);

                await channel.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: queueName,
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
}
