using System.Text;
using System.Text.Json;
using ConsumerAPI;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ProducerAPI;

public interface IRabbitMqService
{
    void SendMessage(object obj);

    void SendMessage(string message);
}

public class RabbitMqService : IRabbitMqService
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly RabbitMqSettings _settings;
    
    public RabbitMqService(IOptionsMonitor<RabbitMqSettings> optionsMonitor, IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        _settings = optionsMonitor.CurrentValue;
    }
    
    public void SendMessage(object obj)
    {
        var message = JsonSerializer.Serialize(obj);
        SendMessage(message);
    }

    public void SendMessage(string message)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: _settings.QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
            routingKey: _settings.QueueName,
            basicProperties: null,
            body: body);
    }
}