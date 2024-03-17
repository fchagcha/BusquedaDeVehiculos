namespace Bdv.Reservas.Dominio.Entidades
{
    public partial class Vehiculo
    {
        public static readonly Error ErrorNoHayVehiculosDisponibles = new(
            "Localidad.NoHayVehiculosDisponibles",
            "No existe vehiculos disponibles para los criterios ingresados");
    }
}