namespace Bdv.Infraestructura.Data.UnitOfWork
{
    public class UoWCommand(AplicationDbContext dbContext) : IUoWCommand
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

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<bool> ExisteAsync<TEntidad>(
            Expression<Func<TEntidad, bool>> predicado,
            CancellationToken cancellationToken = default) where TEntidad : class, IEntity
        {
            return await dbContext.Set<TEntidad>().AnyAsync(predicado, cancellationToken);
        }

        public async Task<TEntidad> ObtenerAsync<TEntidad>(
            Expression<Func<TEntidad, bool>> predicado,
            CancellationToken cancellationToken = default) where TEntidad : class, IEntity
        {
            var entidad = dbContext.Set<TEntidad>().Local.AsQueryable().FirstOrDefault(predicado);

            if (entidad is null)
            {
                var query = dbContext.Set<TEntidad>().AsQueryable();

                entidad = await query.FirstOrDefaultAsync(predicado, cancellationToken);
            }

            return entidad;
        }

        public async Task<TEntidad> ObtenerPorIdAsync<TEntidad>(params object[] ids) where TEntidad : class, IEntity
        {
            return await dbContext.Set<TEntidad>().FindAsync(ids);
        }


        public TEntidad ObtenerDatosOriginales<TEntidad>(TEntidad entidad) where TEntidad : class, IEntity
        {
            var entry = dbContext.Entry(entidad);

            if (entry is null)
                return null;

            return entry.OriginalValues.ToObject() as TEntidad;
        }

        public Task EjecutaStoredProcedure(string nombre, params object[] parametros)
        {
            throw new NotImplementedException();
            //var parametrosSql = parametros.Select(p => new SqlParameter { Value = p }).ToArray();
            //await dbContext.Database.ExecuteSqlRawAsync($"EXEC {nombre}", parametrosSql);
        }
        public Task<TResult> EjecutaStoredProcedure<TResult>(string nombre, params object[] parametros)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntidad> AgregarAsync<TEntidad>(TEntidad entidad) where TEntidad : class, IEntity, IAggregateRoot
        {
            var entityEntry = await dbContext.Set<TEntidad>().AddAsync(entidad);
            return entityEntry.Entity;
        }
        public Task EliminarAsync<TEntidad>(TEntidad entidad) where TEntidad : class, IEntity
        {
            throw new NotImplementedException();
        }
        public async Task AgregarVariosAsync<TEntidad>(IEnumerable<TEntidad> entidades) where TEntidad : class, IEntity, IAggregateRoot
        {
            await dbContext.Set<TEntidad>().AddRangeAsync(entidades);
        }
    }
}