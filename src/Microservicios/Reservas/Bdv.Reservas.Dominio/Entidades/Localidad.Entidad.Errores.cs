namespace Bdv.Reservas.Dominio.Entidades
{
    public partial class Localidad
    {
        public static readonly Error ErrorLocalidadRecogidaNoExiste = new(
            "Localidad.NoExiste",
            "Localidad recogida ingresada no existe");

        public static readonly Error ErrorLocalidadDevolucionNoExiste = new(
            "Localidad.NoExiste",
            "Localidad devolución ingresada no existe");
    }
}
