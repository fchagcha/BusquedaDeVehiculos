using Exceptionless;
using Fabrela.Infraestructura.Messaging.DependencyInjection;
using Integracion.Comun.Enums;
using Integracion.Infraestructura.Enums;
using MassTransit;

namespace Bdv.EnvioCorreo.Api.Configure
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection ConfigureServices<TConsumer>(this IServiceCollection services,
            IConfiguration configuration,
            EventBusProvider eventBusProvider) where TConsumer : class, IConsumer
        {
            services.AddExceptionless(configuration)
                    .CofigureIntegrationForJobs(configuration, DatabaseType.MySQL)
                    .AddEventBusServiceConsumer<TConsumer>(
                        configuration,
                        eventBusProvider);

            return services;
        }
    }
}
