namespace Bdv.Infraestructura.Data.Interceptors
{
    public sealed class UpdateAddedInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context is not null)
                ModificastadoEntidadAgregada(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void ModificastadoEntidadAgregada(DbContext context)
        {
            var entidades =
                context
                .ChangeTracker
                .Entries<IEntity>()
                .Where(x => x.Entity.Version is null);

            if (!entidades.Any())
                return;

            foreach (var entry in entidades)
                entry.State = EntityState.Added;
        }
    }
}