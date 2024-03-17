using Bdv.BackgroundPublisherJob.Api.Jobs;
using Exceptionless;
using Fabrela.Infraestructura.Messaging.DependencyInjection;
using Integracion.Comun.Enums;
using Integracion.Infraestructura.Enums;
using Quartz;

namespace Bdv.BackgroundPublisherJob.Api.Configure
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services,
            IConfiguration configuration,
            DatabaseType databaseType,
            EventBusProvider eventBusProvider)
        {
            services.AddExceptionless(configuration)
                    .CofigureIntegrationForJobs(configuration, databaseType)
                    .AddEventBusServicePublisher(configuration, eventBusProvider)
                    .AddQuartzService();

            return services;
        }

        private static IServiceCollection AddQuartzService(this IServiceCollection services)
        {
            services.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

                configure.AddJob<ProcessOutboxMessagesJob>(jobKey)
                         .AddTrigger(trigger =>
                            trigger
                            .ForJob(jobKey)
                            .WithSimpleSchedule(schedule =>
                                schedule
                                .WithIntervalInSeconds(10)
                                .RepeatForever()));

                configure.UseMicrosoftDependencyInjectionJobFactory();
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            return services;
        }
    }
}
