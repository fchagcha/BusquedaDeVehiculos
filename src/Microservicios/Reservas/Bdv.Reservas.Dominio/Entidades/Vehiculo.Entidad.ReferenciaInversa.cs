namespace Bdv.Reservas.Dominio.Entidades
{
    public partial class Vehiculo
    {
        private HashSet<Reserva> _reservas;
        [InverseProperty(nameof(Reserva.Vehiculo))]
        public IEnumerable<Reserva> Reservas
        {
            get { LazyLoader.Load(this, ref _reservas); return _reservas; }
        }
    }
}