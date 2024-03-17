using Integracion.Infraestructura.Settings;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Integracion.Infraestructura.DependencyInjection
{
    internal static class AzureServiceBus
    {
        public static IServiceCollection AddAzurePublisher(this IServiceCollection services,
            EventBusSettings eventBusSettings)
        {
            var azureServiceBusSettings = eventBusSettings.AzureServiceBusSettings ?? throw new InvalidOperationException("AzureServiceBus settings no ha sido configurado.");

            services.AddMassTransit(x =>
            {
                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(azureServiceBusSettings.ConnectionString);
                });
            });

            return services;
        }
        public static IServiceCollection AddAzurePublisherConsumer<TConsumer>(this IServiceCollection services,
            EventBusSettings eventBusSettings) where TConsumer : class, IConsumer
        {
            var azureServiceBusSettings = eventBusSettings.AzureServiceBusSettings ?? throw new InvalidOperationException("AzureServiceBus settings no ha sido configurado.");

            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumer<TConsumer>();

                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(azureServiceBusSettings.ConnectionString);

                    cfg.ReceiveEndpoint(eventBusSettings.Queue, e =>
                    {
                        e.ConfigureConsumer<TConsumer>(context);
                    });
                });
            });

            return services;
        }
    }
}