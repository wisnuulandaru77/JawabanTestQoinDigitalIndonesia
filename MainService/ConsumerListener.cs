namespace MainService;

public class ConsumerListener(IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private IConfiguration configuration;


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        GetConfiguration();
        using var connectionFactoryScope = serviceScopeFactory.CreateScope();
        ConsumerConfiguration(ChannelConfiguration(connectionFactoryScope));

        return Task.CompletedTask;
    }

    private void GetConfiguration()
    {
        using var connectionFactoryScope = serviceScopeFactory.CreateScope();
        configuration = connectionFactoryScope.ServiceProvider.GetRequiredService<IConfiguration>();
    }

    private IModel ChannelConfiguration(IServiceScope connectionFactoryScope)
    {
        var connectionFactory = connectionFactoryScope.ServiceProvider.GetRequiredService<IConnectionFactory>();        

        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
                              queue: configuration.GetSection(GlobalVariable.QueueName).Value,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null
                             );

        return _channel;
    }

    private void ConsumerConfiguration(IModel channel)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (obj, bde) =>
        {
            await ModifiedDb(Encoding.UTF8.GetString(bde.Body.ToArray()));
        };

        _channel.BasicConsume(
                              queue: configuration.GetSection(GlobalVariable.QueueName).Value,
                              autoAck: true,
                              consumer: consumer
                             );
    }

    private async Task ModifiedDb(string message)
    {
        var baseMessageBrokerDTO = JsonSerializer.Deserialize<BaseMessageBrokerDTO>(message);

        using var test01Scope = serviceScopeFactory.CreateScope();

        var test01Service = test01Scope.ServiceProvider.GetRequiredService<ITest01Service>();

        switch (baseMessageBrokerDTO.command.ToLower())
        {
            case GlobalVariable.CreateCommand:
                var dtoCreate = JsonSerializer.Deserialize<MessageBrokerDTO<ReqCreateTest01DTO>>(message);
                var test01Create = new ReqCreateTest01DTO
                {
                    Nama = dtoCreate.data.Nama,
                    Status = dtoCreate.data.Status
                };

                _ = await test01Service.CreateAsync(test01Create);
                Console.WriteLine("RabbitMQ : Data is created");
                break;
            case GlobalVariable.UpdateCommand:
                var dtoUpdate = JsonSerializer.Deserialize<MessageBrokerDTO<ReqUpdateTest01DTO>>(message);
                var test01Update = new ReqUpdateTest01DTO
                {
                    Id = dtoUpdate.data.Id,
                    Nama = dtoUpdate.data.Nama,
                    Status = dtoUpdate.data.Status
                };

                _ = await test01Service.UpdateAsync(test01Update);
                Console.WriteLine("RabbitMQ : Data is updated");
                break;
            case GlobalVariable.DeleteCommand:
                var dtoDelete = JsonSerializer.Deserialize<MessageBrokerDTO<BaseTest01DTO>>(message);

                _ = await test01Service.DeleteAsync(dtoDelete.data.Id);
                Console.WriteLine("RabbitMQ : Data is deleted");
                break;
        }
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}
