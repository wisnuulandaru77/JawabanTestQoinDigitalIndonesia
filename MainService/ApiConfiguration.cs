namespace MainService;

public static class ApiConfiguration
{
    public static void RegisterDependencyInjection(this IServiceCollection serviceCollections)
    {
        serviceCollections.AddControllers();
        serviceCollections.AddEndpointsApiExplorer();
        serviceCollections.AddSwaggerGen();

        serviceCollections.AddScoped<ITest01Service, Test01Service>();
        serviceCollections.AddScoped<IMessageProducerService, MessageProducerService>();
        serviceCollections.AddHostedService<ConsumerListener>();
        serviceCollections.AddScoped<IConnectionFactory>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return new ConnectionFactory { HostName = configuration.GetSection(GlobalVariable.RabbitMqHostName).Value };
        });
    }

    public static void DatabaseConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options => {

            var connectionString = builder.Configuration.GetConnectionString(GlobalVariable.DbConStringName);
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });
    }

    public static void MiddlewareConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
