using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace ContactManagerCS.Common.Loggers;

public class RabbitMQLogger : ICustomLogger
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;

    public RabbitMQLogger(IOptions<RabbitMQOptions> options)
    {
        var rabbitMQOptions = options.Value;

        var factory = new ConnectionFactory() { HostName = rabbitMQOptions.Hostname, Port = rabbitMQOptions.Port };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _queueName = rabbitMQOptions.QueueName;

        _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public void Log(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
    }
}
