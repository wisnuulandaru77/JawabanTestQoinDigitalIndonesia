namespace MainService.Services;

public interface IMessageProducerService
{
    IActionResult SendingMessage<T>(T message);
}

public class MessageProducerService(IConnectionFactory connectionFactory,IConfiguration configuration) : IMessageProducerService
{
    public IActionResult SendingMessage<T>(T message)
    {
        using var channel = CreateConnection()
                           .CreateModel();

        ChannelConfiguration(channel);      
        PublishMessage(message,channel);

        channel.Close();

        return new OkObjectResult("Sending data to RabbitMQ was succes");
    }

    private IConnection CreateConnection()
    {
        return connectionFactory.CreateConnection();
    }

    private void ChannelConfiguration(IModel channel)
    {
        channel.QueueDeclare(
                             queue: configuration.GetSection(GlobalVariable.QueueName).Value,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null
                            );
    }

    private void PublishMessage<T>(T message, IModel channel)
    {
        channel.BasicPublish(
                              exchange: "",
                              routingKey: configuration.GetSection(GlobalVariable.RabbitMqRoutingKey).Value,
                              basicProperties: null,
                              body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message))
                            );
    }
}
