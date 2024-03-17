using Bdv.Cqrs.Interfaces;
using Integracion.Comun.Events;

namespace Integracion.Dto.Events
{
    public class MicroservicioCorreoNotificadoEvent : IIntegrationEvent
    {
        public Guid Id { get; set; }
        public ICommand Content { get; set; }
    }
}