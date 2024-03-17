using Bdv.Configuracion.Microservicios;
using Integracion.Comun.Enums;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureServices(
        builder.Configuration,
        esQuery: false,
        DatabaseType.MySQL);

builder
    .Build()
    .ConfigurePipeline(builder.Configuration)
    .Run();