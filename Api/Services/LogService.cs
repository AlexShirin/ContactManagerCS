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
    private readonly string _queueNameError;
    private readonly string _exchangeName;
    private readonly IServiceScopeFactory _scopeFactory;

    public LogService(IOptions<RabbitMQOptions> options, IServiceScopeFactory scopeFactory)
    {
        var rabbitMQOptions = options.Value;
        _queueName = rabbitMQOptions.QueueName;
        _queueNameError = _queueName + "_error";
        _exchangeName = rabbitMQOptions.ExchangeName;
        _scopeFactory = scopeFactory;

        var factory = new ConnectionFactory() { HostName = rabbitMQOptions.Hostname, Port = rabbitMQOptions.Port };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct);

        _channel.QueueDeclare(queue: _queueNameError, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var args = new Dictionary<string, object>();
        args.Add("x-dead-letter-exchange", _queueNameError);
        _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: args);

        _channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: _queueName);
        _channel.QueueBind(queue: _queueNameError, exchange: _exchangeName, routingKey: _queueNameError);
    }

    public void Start()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            try
            {
                SaveLog(message).GetAwaiter().GetResult();
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                HandleError(ea.DeliveryTag, body, ex);
            }
        };
        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
    }

    private void HandleError(ulong deliveryTag, byte[] body, Exception exception)
    {
        //throw new NotImplementedException();
        var errorMessage = Encoding.UTF8.GetString(body);
        var errorDetails = $"Exception: {exception.Message} | {errorMessage}";
        var errorBody = Encoding.UTF8.GetBytes(errorDetails);

        _channel.BasicPublish(exchange: _exchangeName, routingKey: _queueNameError, basicProperties: null, body: errorBody);

        _channel.BasicNack(deliveryTag, false, false);
    }

    private async Task SaveLog(string message)
    {
        //throw new NotImplementedException();
        var log = new Log { Message = message };
        using var scope = _scopeFactory.CreateScope();
        var logRepository = scope.ServiceProvider.GetRequiredService<ILogRepository>();
        await logRepository.Add(log);
    }
}