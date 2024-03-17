namespace Bdv.Reservas.Dominio.Entidades
{
    public partial class Reserva
    {
        public static Reserva Crear(
            Guid idVehiculo,
            Guid idLocalidadRecogida,
            Guid idLocalidadDevolucion,
            string nombreConductor,
            string correoElectronicoConductor,
            DateTime fechaRecogida,
            DateTime fechaDevolucion,
            decimal tarifaTotal)
        {
            return new Reserva(
                idVehiculo,
                idLocalidadRecogida,
                idLocalidadDevolucion,
                nombreConductor,
                correoElectronicoConductor,
                fechaRecogida,
                fechaDevolucion,
                tarifaTotal);
        }
    }
}