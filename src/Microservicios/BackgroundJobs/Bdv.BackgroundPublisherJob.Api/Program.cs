using Bdv.BackgroundPublisherJob.Api.Configure;
using Exceptionless;
using Integracion.Comun.Enums;
using Integracion.Infraestructura.Enums;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureServices(
        builder.Configuration,
        DatabaseType.MySQL,
        EventBusProvider.RabbitMq)
    .AddEndpointsApiExplorer();

var app = builder.Build();
app.UseExceptionless(builder.Configuration);
app.UseHttpsRedirection();
app.Run();
