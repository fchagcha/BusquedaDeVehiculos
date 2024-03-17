using Bdv.Cqrs.Interfaces;

namespace Bdv.Reservas.Aplicacion.Dto.Command
{
    public record CrearReservaRequest(
        Guid IdVehiculo,
        Guid IdLocalidadRecogida,
        Guid IdLocalidadDevolucion,
        string NombreConductor,
        string CorreoElectronicoConductor,
        DateTime FechaRecogida,
        DateTime FechaDevolucion,
        decimal TarifaTotal) : ICommand;
}