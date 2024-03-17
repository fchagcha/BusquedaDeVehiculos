using Integracion.Comun.Enums;
using Integracion.Core.InterfacesRepository;
using Integracion.Core.InterfacesServices;
using Integracion.Infraestructura.Persistencia;
using Integracion.Infraestructura.Repository;
using Integracion.Infraestructura.Servicios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fabrela.Infraestructura.Messaging.DependencyInjection
{
    public static partial class ConfigureServices
    {
        public static IServiceCollection CofigureIntegrationForMicroservices(this IServiceCollection services,
            IConfiguration configuration,
            DatabaseType databaseType)
        {
            services.ConfigureOutboxDatabase(configuration, databaseType)
                .ConfigureIntegrationServices()
                .CofigureOutboxRepository();

            return services;
        }
        public static IServiceCollection CofigureIntegrationForJobs(this IServiceCollection services,
            IConfiguration configuration,
            DatabaseType databaseType)
        {
            services.ConfigureOutboxDatabase(configuration, databaseType)
                    .CofigureOutboxRepository();

            return services;
        }

        private static IServiceCollection CofigureOutboxRepository(this IServiceCollection services)
        {
            services.AddScoped<IOutboxRepository, OutboxRepository>();

            return services;
        }
        private static IServiceCollection ConfigureIntegrationServices(this IServiceCollection services)
        {
            services.AddScoped<IIntegrationServices, IntegrationServices>();

            return services;
        }
        private static IServiceCollection ConfigureOutboxDatabase(this IServiceCollection services,
            IConfiguration configuration, DatabaseType databaseType)
        {
            switch (databaseType)
            {
                case DatabaseType.SQLServer:
                    services.ConfigureDataBaseSqlServer(configuration);
                    break;
                case DatabaseType.MySQL:
                    services.ConfigureDataBaseMySql(configuration);
                    break;
            }

            return services;
        }

        private static IServiceCollection ConfigureDataBaseSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OutboxDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("ConnectionStringOutbox")));

            return services;
        }
        private static IServiceCollection ConfigureDataBaseMySql(this IServiceCollection services, IConfiguration configuration)
        {
            var mySqlConnectionStr = configuration.GetConnectionString("ConnectionStringOutbox");
            services.AddDbContext<OutboxDbContext>(options =>
                options.UseMySql(mySqlConnectionStr,
                                 ServerVersion.AutoDetect(mySqlConnectionStr)));

            return services;
        }
    }
}