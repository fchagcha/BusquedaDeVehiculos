using Integracion.Comun.Events;
using Integracion.Core.InterfacesServices;

namespace Integracion.Infraestructura.Servicios
{
    public class IntegrationServices : IIntegrationServices
    {
        private readonly Queue<IIntegrationEvent> _events = new();

        public void AgregarEvento(IIntegrationEvent evento)
        {
            _events.Enqueue(evento);
        }
        public void LimpiarEventos()
        {
            _events.Clear();
        }
        public IEnumerable<IIntegrationEvent> ObtenerEventos()
        {
            return _events.ToArray();
        }
    }
}