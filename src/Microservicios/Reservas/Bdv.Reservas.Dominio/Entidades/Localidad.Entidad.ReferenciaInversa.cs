namespace Bdv.Reservas.Dominio.Entidades
{
    public partial class Localidad
    {
        private HashSet<Vehiculo> _vehiculoLocalidadActuales;
        [InverseProperty(nameof(Vehiculo.LocalidadActual))]
        public IEnumerable<Vehiculo> VehiculoLocalidadActuales
        {
            get { LazyLoader.Load(this, ref _vehiculoLocalidadActuales); return _vehiculoLocalidadActuales; }
        }

        private HashSet<Reserva> _reservaLocalidadRecogidas;
        [InverseProperty(nameof(Reserva.LocalidadRecogida))]
        public IEnumerable<Reserva> ReservaLocalidadRecogidas
        {
            get { LazyLoader.Load(this, ref _reservaLocalidadRecogidas); return _reservaLocalidadRecogidas; }
        }

        private HashSet<Reserva> _reservaLocalidadDevoluciones;
        [InverseProperty(nameof(Reserva.LocalidadDevolucion))]
        public IEnumerable<Reserva> ReservaLocalidadDevoluciones
        {
            get { LazyLoader.Load(this, ref _reservaLocalidadDevoluciones); return _reservaLocalidadDevoluciones; }
        }
    }
}