using ConsumerAPI;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ProducerAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddRabbitMqConnectionFactory(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionFactory, ConnectionFactory>(provider =>
        {
            var settings = provider.GetRequiredService<IOptionsMonitor<RabbitMqSettings>>().CurrentValue;

            return new ConnectionFactory
            {
                HostName = settings.Host,
                UserName = "user",
                Password = "password",
                Port = 5672
            };
        });
    }
}