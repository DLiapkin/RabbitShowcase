using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitShowcase.Basic.Consumer
{
    /// <summary>
    /// Consumer
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
                autoDelete: false,
                arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                Thread.Sleep(1000);

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received: {message}");

                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: true);
            };

            await channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer);

            Console.ReadLine();
        }
    }
}
