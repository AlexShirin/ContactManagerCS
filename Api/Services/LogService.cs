using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using ContactManagerCS.DAL.Database;
using ContactManagerCS.DAL.Entities;
using System.Text;
using ContactManagerCS.Common.Loggers;
using Microsoft.Extensions.Options;
using ContactManagerCS.DAL.Repositories;

namespace ContactManagerCS.Services;

public class LogService : ILogService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;
    private readonly IServiceScopeFactory _scopeFactory;

    public LogService(IOptions<RabbitMQOptions> options, IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        var rabbitMQOptions = options.Value;

        var factory = new ConnectionFactory() { HostName = rabbitMQOptions.Hostname, Port = rabbitMQOptions.Port };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _queueName = rabbitMQOptions.QueueName;

        _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public void Start()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            SaveLog(message).GetAwaiter().GetResult();
        };
        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
    }

    private async Task SaveLog(string message)
    {
        var log = new Log { Message = message };
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ILogRepository>();
        await context.Add(log);
    }
}