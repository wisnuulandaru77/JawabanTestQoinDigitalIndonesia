var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterDependencyInjection();
builder.DatabaseConfiguration();
builder.Build().MiddlewareConfiguration();

