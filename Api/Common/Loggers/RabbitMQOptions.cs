namespace ContactManagerCS.Common.Loggers;

public class RabbitMQOptions
{
    public string Hostname { get; set; }

    public int Port { get; set; }

    public string QueueName { get; set; }
}
//docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.13-management