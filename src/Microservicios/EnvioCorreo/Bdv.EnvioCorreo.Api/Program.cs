using Bdv.EnvioCorreo.Api.Configure;
using Bdv.EnvioCorreo.Api.Consumers;
using Exceptionless;
using Integracion.Comun.Enums;
using Integracion.Infraestructura.Enums;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureServices<EnviarCorreoConsumer>(
        builder.Configuration,
        DatabaseType.MySQL,
        EventBusProvider.RabbitMq)
    .AddEndpointsApiExplorer();

var app = builder.Build();
app.UseExceptionless(builder.Configuration);
app.UseHttpsRedirection();
app.Run();
