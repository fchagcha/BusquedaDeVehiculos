using Integracion.Infraestructura.Settings;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Integracion.Infraestructura.DependencyInjection
{
    internal static class RabbitMqServiceBus
    {
        public static IServiceCollection AddRabbitMqPublisher(this IServiceCollection services,
            EventBusSettings eventBusSettings)
        {
            var rabbitMqSettings = eventBusSettings.RabbitMqSettings ?? throw new InvalidOperationException("RabbitMQ settings no ha sido configurado.");

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.HostName, h =>
                    {
                        h.Username(rabbitMqSettings.UserNameRabbitMq);
                        h.Password(rabbitMqSettings.Password);
                    });
                });
            });

            return services;
        }
        public static IServiceCollection AddRabbitMqPublisherConsumer<TConsumer>(this IServiceCollection services,
            EventBusSettings eventBusSettings) where TConsumer : class, IConsumer
        {
            var rabbitMqSettings = eventBusSettings.RabbitMqSettings ?? throw new InvalidOperationException("RabbitMQ settings no ha sido configurado.");

            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumer<TConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.HostName, h =>
                    {
                        h.Username(rabbitMqSettings.UserNameRabbitMq);
                        h.Password(rabbitMqSettings.Password);
                    });

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
