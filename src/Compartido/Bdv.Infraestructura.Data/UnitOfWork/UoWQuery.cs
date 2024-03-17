namespace Bdv.Infraestructura.Data.UnitOfWork
{
    public class UoWQuery(AplicationDbContext dbContext) : IUoWQuery
    {
        private volatile bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                    dbContext?.Dispose();

                _disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IQueryable<TEntidad> Filtrar<TEntidad>(
            Expression<Func<TEntidad, bool>> predicado = null,
            CancellationToken cancellationToken = default) where TEntidad : class, IEntity
        {
            var query = dbContext.Set<TEntidad>().AsQueryable();

            if (predicado is not null)
                query = query.Where(predicado);

            return query;
        }

        public IQueryable<TResult> Proyectar<TEntidad, TResult>(
            Expression<Func<TEntidad, TResult>> selector,
            CancellationToken cancellationToken = default) where TEntidad : class, IEntity
        {
            var query = dbContext.Set<TEntidad>().AsQueryable();

            return query.Select(selector);
        }

        public IQueryable<TEntidad> ConsultaSql<TEntidad>(
            string consulta,
            params object[] parametros) where TEntidad : class, IEntity
        {
            return dbContext.Set<TEntidad>().FromSqlRaw(consulta, parametros);
        }

        public async Task<List<TEntidad>> ListarAsync<TEntidad>(
            Expression<Func<TEntidad, bool>> predicado = null,
            CancellationToken cancellationToken = default) where TEntidad : class, IEntity
        {
            var query = dbContext.Set<TEntidad>().AsQueryable();

            if (predicado is not null)
                query = query.Where(predicado);

            return await query.ToListAsync();
        }

        public async Task<bool> ExisteAsync<TEntidad>(
            Expression<Func<TEntidad, bool>> predicado,
            CancellationToken cancellationToken = default) where TEntidad : class, IEntity
        {
            return await dbContext.Set<TEntidad>().AnyAsync(predicado);
        }

        public async Task<TEntidad> ObtenerAsync<TEntidad>(
            Expression<Func<TEntidad, bool>> predicado,
            CancellationToken cancellationToken = default) where TEntidad : class, IEntity
        {
            var entidad = dbContext.Set<TEntidad>().Local.AsQueryable().FirstOrDefault(predicado);

            if (entidad is null)
            {
                var query = dbContext.Set<TEntidad>().AsQueryable();

                entidad = await query.FirstOrDefaultAsync(predicado);
            }

            return entidad;
        }

        public async Task<TEntidad> ObtenerPorIdAsync<TEntidad>(params object[] ids) where TEntidad : class, IEntity
        {
            return await dbContext.Set<TEntidad>().FindAsync(ids);
        }

        public Task<TResult> EjecutaStoredProcedure<TResult>(string nombre, params object[] parametros)
        {
            throw new NotImplementedException();
        }
    }
}