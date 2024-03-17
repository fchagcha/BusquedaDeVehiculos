using Bdv.Configuracion.Microservicios;
using Integracion.Comun.Enums;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureServices(
        builder.Configuration,
        esQuery: true,
        DatabaseType.MySQL,
        typeof(Program).Assembly.GetName().Name);

builder
    .Build()
    .ConfigurePipeline(builder.Configuration)
    .Run();