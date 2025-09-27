using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQShowcase.WorkloadBalance.ConsumerTwo;

internal class ConsumerTwo
{
    static async Task Main(string[] args)
    {
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

        Console.WriteLine(" [*] Consumer Two [*]");
        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            Thread.Sleep(500);

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
