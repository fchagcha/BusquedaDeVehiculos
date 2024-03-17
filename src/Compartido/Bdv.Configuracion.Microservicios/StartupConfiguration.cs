using Microsoft.OpenApi.Models;

namespace Bdv.Configuracion.Microservicios
{
    public static class StartupConfiguration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services,
            IConfiguration configuration,
            bool esQuery,
            DatabaseType databaseType,
            string nombreMicroservicio = "")
        {
            services
                .ConfigureControllers()
                .ConfigureDatabase<AplicationDbContext>(configuration, esQuery, databaseType)
                .AddInfraestructuraServices(configuration, esQuery)
                .CofigureIntegrationForMicroservices(configuration, databaseType)
                .ConfigureCors();

            if (nombreMicroservicio != "")
                services.AddSwagger(nombreMicroservicio);
            else
                services.AddSwaggerGen();

            return services;
        }

        private static IServiceCollection ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            return services;
        }
        private static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            return services;
        }
        private static IServiceCollection AddSwagger(this IServiceCollection services, string nombreMicroservicio)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // Configura Swagger para usar el archivo XML
                var xmlFile = $"{nombreMicroservicio}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }
    }
}