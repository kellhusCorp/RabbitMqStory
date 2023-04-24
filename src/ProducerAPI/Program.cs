using Microsoft.Extensions.Options;
using ProducerAPI;
using ProducerAPI.Settings;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));
builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();
builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>(provider =>
{
    var settings = provider.GetRequiredService<IOptionsMonitor<RabbitMqSettings>>()?.CurrentValue;

    return new ConnectionFactory
    {
        HostName = settings.Host,
        UserName = "user",
        Password = "password",
        Port = 5672
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();