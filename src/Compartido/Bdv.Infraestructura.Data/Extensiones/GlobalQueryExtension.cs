namespace Fabrela.Infraestructura.Data.Extensiones
{
    public static partial class ModelBuilderExtension
    {
        private static readonly MethodInfo GlobalQueryMethodActivo =
            typeof(ModelBuilderExtension)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Single(x => x.IsGenericMethod &&
                         x.Name == nameof(GlobalQueryActivo));

        public static ModelBuilder AplicarFiltrosGlobales<T>(this ModelBuilder modelBuilder) where T : class, IEntity
        {
            var entidades =
                modelBuilder.Model
                .GetEntityTypes()
                .Where(e => typeof(T).IsAssignableFrom(e.ClrType));

            foreach (var entidad in entidades)
            {
                var tipo = entidad.ClrType;

                var method = GlobalQueryMethodActivo.MakeGenericMethod(tipo);
                method.Invoke(null, [modelBuilder]);
            }

            return modelBuilder;
        }

        private static void GlobalQueryActivo<T>(ModelBuilder modelBuilder) where T : class, IEntity
        {
            modelBuilder.Entity<T>().HasQueryFilter(x => !x.EstaBorrado);
        }
    }
}