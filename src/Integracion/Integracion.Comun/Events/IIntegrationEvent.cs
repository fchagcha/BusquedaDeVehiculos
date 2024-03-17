using Bdv.Cqrs.Interfaces;

namespace Integracion.Comun.Events
{
    public interface IIntegrationEvent
    {
        public Guid Id { get; set; }
        public ICommand Content { get; set; }
    }
}