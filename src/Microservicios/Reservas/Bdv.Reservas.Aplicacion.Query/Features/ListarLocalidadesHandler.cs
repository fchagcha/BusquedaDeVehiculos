namespace Bdv.Reservas.Aplicacion.Query.Features
{
    internal class ListarLocalidadesHandler(IProvider provider) : IQueryHandler<ListarLocalidadesRequest, List<ListarLocalidadesResponse>>
    {
        private readonly IUoWQuery _query = provider.ObtenerUnitOfWork<IUoWQuery>();

        public async Task<Result<List<ListarLocalidadesResponse>>> Handle(ListarLocalidadesRequest request, CancellationToken cancellationToken)
            => await
                _query
                .Filtrar<Localidad>()
                .Select(x => new ListarLocalidadesResponse(
                    x.Id,
                    x.Nombre,
                    x.Mercado))
                .ToListAsync();
    }
}