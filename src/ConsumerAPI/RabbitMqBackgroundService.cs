using System.Text;
using ConsumerAPI.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsumerAPI;

public class RabbitMqBackgroundService : BackgroundService
{
    private readonly IConsumerService _consumerService;

    public RabbitMqBackgroundService(IConsumerService consumerService)
    {
        _consumerService = consumerService;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consumerService.ReadMessages();
    }
}