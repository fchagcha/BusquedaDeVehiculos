namespace Bdv.Reservas.Aplicacion.Query.Validadores
{
    public class ListarVehiculosDisponiblesValidator : AbstractValidator<ListarVehiculosDisponiblesRequest>
    {
        public ListarVehiculosDisponiblesValidator()
        {
            RuleFor(request => request.IdLocalidadRecogida)
                .NotEmpty()
                .WithMessage("Localidad Recogida es obligatorio.");

            RuleFor(request => request.FechaDeRecogida)
                .NotEmpty()
                .WithMessage("Fecha de recogida es obligatorio.");

            RuleFor(request => request.FechaDeDevolucion)
                .NotEmpty()
                .WithMessage("Fecha de devolución es obligatorio.");
        }
    }
}
