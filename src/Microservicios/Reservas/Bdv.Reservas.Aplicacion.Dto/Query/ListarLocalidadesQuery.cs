using Bdv.Cqrs.Interfaces;

namespace Bdv.Reservas.Aplicacion.Dto.Query
{
    public record ListarLocalidadesResponse(
        Guid IdLocalidad,
        string Nombre,
        string Mercado);
    public record ListarLocalidadesRequest() : IQuery<List<ListarLocalidadesResponse>>;
}