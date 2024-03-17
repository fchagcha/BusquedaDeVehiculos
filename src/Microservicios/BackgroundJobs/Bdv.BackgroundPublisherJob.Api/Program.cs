using Bdv.BackgroundPublisherJob.Api.Configure;
using Exceptionless;
using Integracion.Infraestructura.Enums;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureServices(builder.Configuration, EventBusProvider.RabbitMq)
    .AddEndpointsApiExplorer();

var app = builder.Build();
app.UseExceptionless(builder.Configuration);
app.UseHttpsRedirection();
app.Run();
