using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace ContactManagerCS.Common.Loggers;

public class RabbitMQLogger : ICustomLogger
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;
    private readonly string _exchangeName;

    public RabbitMQLogger(IOptions<RabbitMQOptions> options)
    {
        var rabbitMQOptions = options.Value;
        _queueName = rabbitMQOptions.QueueName;
        _exchangeName = rabbitMQOptions.ExchangeName;

        var factory = new ConnectionFactory() { HostName = rabbitMQOptions.Hostname, Port = rabbitMQOptions.Port };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct);

        //_channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public void Log(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        //_channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
        _channel.BasicPublish(exchange: _exchangeName, routingKey: _queueName, basicProperties: null, body: body);
    }
}
