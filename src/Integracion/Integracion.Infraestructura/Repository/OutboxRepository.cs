using Integracion.Core.Enums;
using Integracion.Core.InterfacesRepository;
using Integracion.Core.Outbox;
using Integracion.Infraestructura.Persistencia;
using System.Linq.Expressions;

namespace Integracion.Infraestructura.Repository
{
    public class OutboxRepository(OutboxDbContext dbContext) : IOutboxRepository
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

        public IQueryable<OutboxMessage> Filtrar(Expression<Func<OutboxMessage, bool>> predicado)
        {
            return dbContext.Set<OutboxMessage>().Where(predicado);
        }

        public async Task<OutboxMessage> ObtenerPorIdAsync(object id)
        {
            return await dbContext.Set<OutboxMessage>().FindAsync(id);
        }
        public async Task<bool> EstaProcesadoAsync(object id)
        {
            var outbox = await ObtenerPorIdAsync(id);

            return outbox.Status == Status.Procesado;
        }

        public async Task<OutboxMessage> AgregarAsync(OutboxMessage entidad)
        {
            var entityEntry = await dbContext.Set<OutboxMessage>().AddAsync(entidad);
            return entityEntry.Entity;
        }
        public async Task AgregarVariosAsync(IEnumerable<OutboxMessage> entidades)
        {
            await dbContext.Set<OutboxMessage>().AddRangeAsync(entidades);
        }

        public async Task<int> SalvarCambiosAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}