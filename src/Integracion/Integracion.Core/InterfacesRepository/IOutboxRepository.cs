using Integracion.Core.Outbox;
using System.Linq.Expressions;

namespace Integracion.Core.InterfacesRepository
{
    public interface IOutboxRepository : IDisposable
    {
        IQueryable<OutboxMessage> Filtrar(Expression<Func<OutboxMessage, bool>> predicado);

        Task<bool> EstaProcesadoAsync(object id);
        Task<OutboxMessage> ObtenerPorIdAsync(object id);
        Task<OutboxMessage> AgregarAsync(OutboxMessage entidad);
        Task AgregarVariosAsync(IEnumerable<OutboxMessage> entidades);
        Task<int> SalvarCambiosAsync(CancellationToken cancellationToken = default);
    }
}