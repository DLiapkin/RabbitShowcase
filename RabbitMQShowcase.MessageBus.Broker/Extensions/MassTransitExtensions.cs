using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQShowcase.MessageBus.Broker.Abstraction;
using System.Reflection;

namespace RabbitMQShowcase.MessageBus.Broker.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMessageBus(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly? scanAssembly = null)
    {
        var assembly = scanAssembly ?? Assembly.GetCallingAssembly();

        services.AddMassTransit(x =>
        {
            x.AddConsumers(assembly);
            x.SetKebabCaseEndpointNameFormatter();
            x.UsingRabbitMq((context, cfg) =>
            {
                ConfigureRabbitMq(cfg, configuration);
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IMessageBus, MessageBus>();

        return services;
    }

    private static void ConfigureRabbitMq(
        IRabbitMqBusFactoryConfigurator cfg,
        IConfiguration configuration)
    {
        var host = configuration["RabbitMQ:Host"] ?? "localhost";
        var username = configuration["RabbitMQ:Username"] ?? "guest";
        var password = configuration["RabbitMQ:Password"] ?? "guest";

        cfg.Host(host, h =>
        {
            h.Username(username);
            h.Password(password);
        });
    }
}
