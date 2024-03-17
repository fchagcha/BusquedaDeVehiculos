namespace Bdv.Infraestructura.Data.Interceptors
{
    public sealed class OutboxInterceptor(IProvider provider) : SaveChangesInterceptor
    {
        private readonly IIntegrationServices _integrationServices = provider.ObtenerServicio<IIntegrationServices>();
        private readonly IOutboxRepository _outboxRepository = provider.ObtenerServicio<IOutboxRepository>();

        public override async ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context is not null)
                await AgregaOutboxAsync(cancellationToken);

            return result;
        }

        private async Task AgregaOutboxAsync(CancellationToken cancellationToken = default)
        {
            var outboxMessages =
                _integrationServices
                .ObtenerEventos()
                .Select(evento => new OutboxMessage(evento));

            await _outboxRepository.AgregarVariosAsync(outboxMessages);
            await _outboxRepository.SalvarCambiosAsync(cancellationToken);
        }
    }
}