namespace MainService;

public class GlobalVariable
{
    public const string DbConStringName = "DefaultConnection";

    public const string QueueName = "RabbitMQ:QueueName";

    public const string RabbitMqRoutingKey = QueueName;

    public const string RabbitMqHostName = "RabbitMQ:Host";

    public const string CreateCommand = "create";

    public const string UpdateCommand = "update";

    public const string DeleteCommand = "delete";

    public const string RoutingForId = "{id}";

    public const string RoutingForPaging = "{page_number}/{page_size}";
}
