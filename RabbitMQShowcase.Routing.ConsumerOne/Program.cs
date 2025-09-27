using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQShowcase.Routing.ConsumerOne;

internal class Program
{
    static async Task Main(string[] args)
    {
        var exchangeName = "direct-logs";
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            autoDelete: true,
            type: ExchangeType.Direct);

        var queue = await channel.QueueDeclareAsync();
        await channel.QueueBindAsync(queue: queue.QueueName, exchange: exchangeName, routingKey: "Info");

        Console.WriteLine(" [*] Consumer One [*]");
        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            Thread.Sleep(500);

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received: {message}");

            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(
            queue: queue.QueueName,
            autoAck: true,
            consumer: consumer);

        Console.ReadLine();
    }
}
