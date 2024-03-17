using Integracion.Infraestructura.DependencyInjection;
using Integracion.Infraestructura.Enums;
using Integracion.Infraestructura.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fabrela.Infraestructura.Messaging.DependencyInjection
{
    public static partial class ConfigureServices
    {
        public static IServiceCollection AddEventBusServicePublisher(this IServiceCollection services,
            IConfiguration configuration,
            EventBusProvider eventBusProvider)
        {
            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();
            _ = eventBusSettings ?? throw new InvalidOperationException("Event Bus Settings no ha sido configurado.");

            switch (eventBusProvider)
            {
                case EventBusProvider.RabbitMq:
                    services.AddRabbitMqPublisher(eventBusSettings);
                    break;
                case EventBusProvider.AzureServiceBus:
                    services.AddAzurePublisher(eventBusSettings);
                    break;
                default:
                    throw new InvalidOperationException("Proveedor de EventBus no soportado");
            }

            return services;
        }
        public static IServiceCollection AddEventBusServiceConsumer<TConsumer>(this IServiceCollection services,
            IConfiguration configuration,
            EventBusProvider eventBusProvider) where TConsumer : class, IConsumer
        {
            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();
            _ = eventBusSettings ?? throw new InvalidOperationException("Event Bus Settings no ha sido configurado.");

            switch (eventBusProvider)
            {
                case EventBusProvider.RabbitMq:
                    services.AddRabbitMqPublisherConsumer<TConsumer>(eventBusSettings);
                    break;
                case EventBusProvider.AzureServiceBus:
                    services.AddAzurePublisherConsumer<TConsumer>(eventBusSettings);
                    break;
                default:
                    throw new InvalidOperationException("Proveedor de EventBus no soportado");
            }

            return services;
        }

    }
}