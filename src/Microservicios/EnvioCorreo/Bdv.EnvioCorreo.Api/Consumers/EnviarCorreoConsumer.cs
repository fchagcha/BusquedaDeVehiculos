using Bdv.Cqrs.Interfaces;
using Exceptionless;
using Integracion.Core.Enums;
using Integracion.Core.InterfacesRepository;
using Integracion.Dto.Events;
using MassTransit;
using MediatR;
using Newtonsoft.Json;

namespace Bdv.EnvioCorreo.Api.Consumers
{
    public class EnviarCorreoConsumer(IOutboxRepository outboxRepository, ISender sender) : IConsumer<MicroservicioCorreoNotificadoEvent>
    {
        public async Task Consume(ConsumeContext<MicroservicioCorreoNotificadoEvent> context)
        {
            try
            {
                var evento = context.Message;

                if (evento is null)
                    return;

                if (evento.Content is null)
                    return;

                var estaProcesado = await
                    outboxRepository
                    .EstaProcesadoAsync(evento.Id);

                if (estaProcesado)
                    return;

                ExceptionlessClient
                    .Default
                    .CreateLog($"Envio Correo: Evento de integración Notificado: {evento.Id}")
                    .Submit();

                var outboxMessage = await
                    outboxRepository
                    .ObtenerPorIdAsync(evento.Id);

                if (outboxMessage is null)
                    return;

                var commandType = ObtenerTipo(outboxMessage.TypeRequest);
                var command = JsonConvert.DeserializeObject(outboxMessage.JsonRequest, commandType) as ICommand;

                await sender.Send(command);

                outboxMessage.Status = Status.Procesado;
                outboxMessage.ProcessedAt = DateTime.Now;
                _ = await outboxRepository.SalvarCambiosAsync();

                ExceptionlessClient
                    .Default
                    .CreateLog($"Envio Correo: Evento de integración Consumido: {evento.Id}")
                    .Submit();
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
                AppDomain.CurrentDomain.GetAssemblies()
                .AsParallel()
                .Select(ensamblado => ensamblado.GetType(nombreCompleto))
                .FirstOrDefault(type => type != null);

            return tipo;
        }
    }
}
