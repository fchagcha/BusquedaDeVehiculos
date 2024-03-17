namespace Bdv.Reservas.Aplicacion.Command.Features
{
    public class CrearReservaHandler(IProvider provider) : ICommandHandler<CrearReservaRequest>
    {
        private readonly IUoWCommand _command = provider.ObtenerUnitOfWork<IUoWCommand>();
        private readonly IIntegrationServices _integrationServices = provider.ObtenerServicio<IIntegrationServices>();
        private readonly IMapper _mapper = provider.ObtenerServicio<IMapper>();
        public async Task<Result> Handle(CrearReservaRequest request, CancellationToken cancellationToken)
        {
            var reserva = Reserva.Crear(
                request.IdVehiculo,
                request.IdLocalidadRecogida,
                request.IdLocalidadDevolucion,
                request.NombreConductor,
                request.CorreoElectronicoConductor,
                request.FechaRecogida,
                request.FechaDevolucion,
                request.TarifaTotal);

            _ = await _command.AgregarAsync(reserva);

            var command = _mapper.Map<EnviarCorreoReservaCommand>(reserva);
            var evento = command.ToIntegrationEvent<MicroservicioCorreoNotificadoEvent>();
            _integrationServices.AgregarEvento(evento);

            return Result.Success();
        }
    }
}
