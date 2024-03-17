namespace Bdv.Reservas.Dominio.Entidades
{
    [Table("Vehiculos")]
    public partial class Vehiculo : BaseEntity<Guid>, IComunEntity
    {
        private readonly ILazyLoader LazyLoader;
        public Vehiculo(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        [Required]
        public Guid IdLocalidadActual { get; private set; }

        [Required]
        public string Marca { get; private set; }

        [Required]
        public string Modelo { get; private set; }

        [Required]
        public string Placa { get; private set; }

        [Required]
        public decimal TarifaDiaria { get; set; }

        [Required]
        public TipoVehiculo Tipo { get; private set; }

        [Required]
        public EstadoVehiculo Estado { get; private set; }


        private Localidad _localidadActual;
        [ForeignKey(nameof(IdLocalidadActual))]
        public Localidad LocalidadActual
        {
            get
            {
                LazyLoader.Load(this, ref _localidadActual);
                return _localidadActual;
            }
            set
            {
                _localidadActual = value;
                IdLocalidadActual = value?.Id ?? Guid.Empty;
            }
        }
    }
}