using Bdv.Cqrs.Interfaces;
using Exceptionless;
using Integracion.Comun.Events;
using Integracion.Core.Enums;
using Integracion.Core.InterfacesRepository;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using System.Reflection;

namespace Bdv.BackgroundPublisherJob.Api.Jobs
{
    [DisallowConcurrentExecution]
    public class ProcessOutboxMessagesJob : IJob
    {
        private readonly IOutboxRepository _outboxRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly Assembly[] _assemblies;
        public ProcessOutboxMessagesJob(IOutboxRepository outboxRepository, IPublishEndpoint publishEndpoint)
        {
            _outboxRepository = outboxRepository;
            _publishEndpoint = publishEndpoint;

            var tipoDummy = typeof(Integracion.Dto.Events.MicroservicioCorreoNotificadoEvent);
            _assemblies = AppDomain.CurrentDomain.GetAssemblies();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var outboxMessages = await
                    _outboxRepository
                    .Filtrar(x => x.Status == Status.Pendiente)
                    .OrderBy(x => x.CreatedAt)
                    .Take(50)
                    .ToArrayAsync();

                foreach (var outboxMessage in outboxMessages)
                {
                    var eventType = ObtenerTipo(outboxMessage.IntegrationEventType);
                    var commandType = ObtenerTipo(outboxMessage.TypeRequest);

                    if (commandType != null && eventType != null)
                    {
                        var command = JsonConvert.DeserializeObject(outboxMessage.JsonRequest, commandType) as ICommand;

                        var integrationEvent = Activator.CreateInstance(eventType) as IIntegrationEvent;
                        integrationEvent.Id = outboxMessage.Id;
                        integrationEvent.Content = command;

                        await _publishEndpoint.Publish(integrationEvent, eventType, publishContext =>
                        {
                            publishContext.MessageId = outboxMessage.Id;
                        }, context.CancellationToken);

                        outboxMessage.Status = Status.Publicado;
                    }
                }

                _ = await _outboxRepository.SalvarCambiosAsync();
            }
            catch (Exception ex)
            {
                ExceptionlessClient
                    .Default
                    .CreateException(ex)
                    .Submit();
            }
        }

        private Type ObtenerTipo(string nombreCompleto)
        {
            var tipo =
                _assemblies
                .AsParallel()
                .Select(ensamblado => ensamblado.GetType(nombreCompleto))
                .FirstOrDefault(type => type != null);

            return tipo;
        }
    }
}