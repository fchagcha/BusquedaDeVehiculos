namespace Bdv.Reservas.Aplicacion.Query.Features
{
    public class ListarVehiculosDisponiblesHandler(IProvider provider) : IQueryHandler<ListarVehiculosDisponiblesRequest, List<VehiculosDisponiblesResponse>>
    {
        private readonly IUoWQuery _query = provider.ObtenerUnitOfWork<IUoWQuery>();

        public async Task<Result<List<VehiculosDisponiblesResponse>>> Handle(ListarVehiculosDisponiblesRequest request, CancellationToken cancellationToken)
            => await
                ObtenerMercadoCLienteAsync(request)
                .Bind(mercadoCliente => ObtenerVehiculosDisponiblesAsync(request, mercadoCliente));

        private async Task<Result<List<VehiculosDisponiblesResponse>>> ObtenerVehiculosDisponiblesAsync(
            ListarVehiculosDisponiblesRequest request,
            string mercadoCliente)
        {
            var idsVehiculosReservadosQuery = ObtenerIdsVehiculosReservadosQuery(request, mercadoCliente);
            var vehiculosDisponiblesQuery = ObtenerVehiculosDisponiblesQuery(request, mercadoCliente, idsVehiculosReservadosQuery);

            var vehiculosDisponiblesResponse = await
                vehiculosDisponiblesQuery
                .Select(vehiculo => new VehiculosDisponiblesResponse(
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
                    vehiculo.TarifaDiaria * (request.FechaDeDevolucion - request.FechaDeRecogida).Days))
                .ToListAsync();

            if (vehiculosDisponiblesResponse.Count == 0)
                return Result.Failure<List<VehiculosDisponiblesResponse>>(Vehiculo.ErrorNoHayVehiculosDisponibles);

            return vehiculosDisponiblesResponse;
        }

        private IQueryable<Vehiculo> ObtenerVehiculosDisponiblesQuery(
            ListarVehiculosDisponiblesRequest request,
            string mercadoCliente,
            IQueryable<Guid> idsVehiculosReservadosQuery)
        {
            var vehiculoQuery =
                _query
                .Filtrar<Vehiculo>(
                    x => x.LocalidadActual.Mercado.Equals(mercadoCliente) &&
                        !idsVehiculosReservadosQuery.Contains(x.Id));

            if (request.TipoVehiculo != 0)
            {
                var tipoVehiculo = (TipoVehiculo)request.TipoVehiculo;
                vehiculoQuery = vehiculoQuery.Where(x => x.Tipo == tipoVehiculo);
            }

            return vehiculoQuery;
        }

        private IQueryable<Guid> ObtenerIdsVehiculosReservadosQuery(
            ListarVehiculosDisponiblesRequest request,
            string mercadoCliente)
        {
            var estadosReserva = new List<EstadoReserva> { EstadoReserva.EnProceso, EstadoReserva.Pendiente };

            var vehiculosReservadosQuery =
                _query
                .Filtrar<Reserva>(
                    x => (x.LocalidadRecogida.Mercado.Equals(mercadoCliente) || x.LocalidadDevolucion.Mercado.Equals(mercadoCliente)) &&
                         (x.FechaRecogida <= request.FechaDeDevolucion && x.FechaDevolucion >= request.FechaDeRecogida) &&
                         estadosReserva.Contains(x.Estado));

            if (request.TipoVehiculo != 0)
            {
                var tipoVehiculo = (TipoVehiculo)request.TipoVehiculo;
                vehiculosReservadosQuery =
                    vehiculosReservadosQuery
                    .Where(x => x.Vehiculo.Tipo == tipoVehiculo);
            }

            var idsVehiculosReservadosQuery =
                vehiculosReservadosQuery
                .Select(x => x.IdVehiculo);

            return idsVehiculosReservadosQuery;
        }

        private async Task<Result<string>> ObtenerMercadoCLienteAsync(ListarVehiculosDisponiblesRequest request)
        {
            var localidadRecogida = await
                 _query
                 .ObtenerPorIdAsync<Localidad>(request.IdLocalidadRecogida);

            if (localidadRecogida is null)
                return Result.Failure<string>(Localidad.ErrorLocalidadRecogidaNoExiste);

            if (!request.IdLocalidadDevolucion.IsNullOrEmpty())
            {
                var localidadDevolucion = await
                        _query
                        .ObtenerPorIdAsync<Localidad>(request.IdLocalidadDevolucion);

                if (localidadDevolucion is null)
                    return Result.Failure<string>(Localidad.ErrorLocalidadDevolucionNoExiste);
            }

            return localidadRecogida.Mercado;
        }
    }
}