using Integracion.Comun.Events;

namespace Integracion.Core.InterfacesServices
{
    public interface IIntegrationServices
    {
        void AgregarEvento(IIntegrationEvent evento);
        void LimpiarEventos();
        IEnumerable<IIntegrationEvent> ObtenerEventos();
    }
}