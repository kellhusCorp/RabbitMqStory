using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsumerAPI.Services;

public interface IConsumerService
{
    Task ReadMessages();
}

public class RabbitMqConsumerService : IConsumerService, IDisposable
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<RabbitMqConsumerService> _logger;
    private readonly IModel _model;
    private readonly RabbitMqSettings _settings;
    private readonly IConnection _connection;
    public RabbitMqConsumerService(        
        IConnectionFactory connectionFactory,
        IOptionsMonitor<RabbitMqSettings> settings,
        ILogger<RabbitMqConsumerService> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _settings = settings.CurrentValue;
        _connection = connectionFactory.CreateConnection();
        _model = _connection.CreateModel();
        _model.QueueDeclare(_settings.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }
    
    public Task ReadMessages()
    {
        var consumer = new EventingBasicConsumer(_model);
        consumer.Received += (ch, ea) =>
        {
            var body = ea.Body.ToArray();
            var text = System.Text.Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Получено сообщение: {text}");
            _model.BasicAck(ea.DeliveryTag, false);
        };
        _model.BasicConsume(_settings.QueueName, false, consumer);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (_model.IsOpen)
            _model.Close();
        if (_connection.IsOpen)
            _connection.Close();
    }
}