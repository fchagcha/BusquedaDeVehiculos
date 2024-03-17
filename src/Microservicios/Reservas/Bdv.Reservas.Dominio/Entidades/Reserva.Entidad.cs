namespace Bdv.Reservas.Dominio.Entidades
{
    [Table("Reservas")]
    public partial class Reserva : BaseEntity<Guid>, IAggregateRoot, IComunEntity
    {
        private readonly ILazyLoader LazyLoader;
        public Reserva(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }
        private Reserva(
            Guid idVehiculo,
            Guid idLocalidadRecogida,
            Guid idLocalidadDevolucion,
            string nombreConductor,
            string correoElectronicoConductor,
            DateTime fechaRecogida,
            DateTime fechaDevolucion,
            decimal tarifaTotal)
        {
            IdVehiculo = idVehiculo;
            IdLocalidadRecogida = idLocalidadRecogida;
            IdLocalidadDevolucion = idLocalidadDevolucion;
            NombreConductor = nombreConductor;
            CorreoElectronicoConductor = correoElectronicoConductor;
            FechaRecogida = fechaRecogida;
            FechaDevolucion = fechaDevolucion;
            TarifaTotal = tarifaTotal;
            Estado = EstadoReserva.Pendiente;
        }

        [Required]
        public Guid IdVehiculo { get; private set; }

        [Required]
        public Guid IdLocalidadRecogida { get; private set; }

        [Required]
        public Guid IdLocalidadDevolucion { get; private set; }

        [Required]
        [StringLength(100)]
        public string NombreConductor { get; private set; }

        [Required]
        [StringLength(50)]
        public string CorreoElectronicoConductor { get; private set; }

        [Required]
        public DateTime FechaRecogida { get; private set; }

        [Required]
        public DateTime FechaDevolucion { get; private set; }

        [Required]
        public decimal TarifaTotal { get; set; }

        [Required]
        public EstadoReserva Estado { get; private set; }


        private Vehiculo _vehiculo;
        [ForeignKey(nameof(IdVehiculo))]
        public Vehiculo Vehiculo
        {
            get
            {
                LazyLoader.Load(this, ref _vehiculo);
                return _vehiculo;
            }
            set
            {
                _vehiculo = value;
                IdVehiculo = value?.Id ?? Guid.Empty;
            }
        }

        private Localidad _localidadRecogida;
        [ForeignKey(nameof(IdLocalidadRecogida))]
        public Localidad LocalidadRecogida
        {
            get
            {
                LazyLoader.Load(this, ref _localidadRecogida);
                return _localidadRecogida;
            }
            set
            {
                _localidadRecogida = value;
                IdLocalidadRecogida = value?.Id ?? Guid.Empty;
            }
        }

        private Localidad _localidadDevolucion;
        [ForeignKey(nameof(IdLocalidadDevolucion))]
        public Localidad LocalidadDevolucion
        {
            get
            {
                LazyLoader.Load(this, ref _localidadDevolucion);
                return _localidadDevolucion;
            }
            set
            {
                _localidadDevolucion = value;
                IdLocalidadDevolucion = value?.Id ?? Guid.Empty;
            }
        }
    }
}
