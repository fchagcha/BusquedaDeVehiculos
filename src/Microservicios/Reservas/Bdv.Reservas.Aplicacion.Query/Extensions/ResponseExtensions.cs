namespace Bdv.Reservas.Aplicacion.Query.Extensions
{
    public static class ResponseExtensions
    {
        public static VehiculosDisponiblesResponse ToVehiculoDisponibleResponse(this Vehiculo vehiculo,
            ListarVehiculosDisponiblesRequest request)
        {
            return new VehiculosDisponiblesResponse(
                vehiculo.LocalidadActual.Mercado,
                vehiculo.Id,
                request.IdLocalidadRecogida,
                request.IdLocalidadDevolucion ?? request.IdLocalidadRecogida,
                request.FechaDeRecogida.ToLongDateString(),
                request.FechaDeDevolucion.ToLongDateString(),
                vehiculo.Tipo.ToString(),
                vehiculo.Marca,
                vehiculo.Modelo,
                vehiculo.TarifaDiaria,
                vehiculo.TarifaDiaria * (request.FechaDeDevolucion - request.FechaDeRecogida).Days);
        }
    }
}