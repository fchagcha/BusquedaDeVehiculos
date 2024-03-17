namespace Bdv.Reservas.Dominio.Entidades
{
    [Table("Localidades")]
    public partial class Localidad : BaseEntity<Guid>, IComunEntity
    {
        private readonly ILazyLoader LazyLoader;
        public Localidad(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        [Required]
        public string Nombre { get; private set; }
        [Required]
        public string Mercado { get; private set; }
    }
}