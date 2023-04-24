using Microsoft.AspNetCore.Mvc;

namespace ProducerAPI.Controllers;

[ApiController]
[Route("/api/rabbitmq")]
public class RabbitMqController : ControllerBase
{
    private readonly IRabbitMqService _rabbitMqService;

    public RabbitMqController(IRabbitMqService rabbitMqService)
    {
        _rabbitMqService = rabbitMqService;
    }

    [HttpPost]
    [Route("send")]
    public IActionResult Send(string message)
    {
        _rabbitMqService.SendMessage(message);
        return Ok("Сообщение отправлено");
    }
}