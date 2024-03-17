using Bdv.Cqrs.Interfaces;

namespace Bdv.Reservas.Aplicacion.Dto.Query
{
    public record VehiculosDisponiblesResponse(
        string Mercado,
        Guid IdVehiculo,
        Guid IdLocalidadRecogida,
        Guid IdLocalidadDevolucion,
        string FechaHoraRecogida,
        string FechaHoraDevolucion,
        string TipoVehiculo,
        string Marca,
        string Modelo,
        decimal TarifaDiaria,
        decimal TotalTarifa);
    public record ListarVehiculosDisponiblesRequest(
        Guid IdLocalidadRecogida,
        Guid? IdLocalidadDevolucion,
        DateTime FechaDeRecogida,
        DateTime FechaDeDevolucion,
        int TipoVehiculo) : IQuery<List<VehiculosDisponiblesResponse>>;
}