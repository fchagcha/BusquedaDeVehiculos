using Bdv.Reservas.Aplicacion.Dto.Query;
using Fabrela.FabrelaResult.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bdv.Reservas.Api.Query.Controllers
{
    /// <summary>
    /// Controlador para bisqueda de vehiculos disponibles.
    /// </summary>
    /// <param></param>
    [ApiController]
    [Route("[controller]/[action]")]
    public class ReservasController(ISender sender) : ControllerBase
    {
        /// <summary>
        /// Obtiene vehiculos disponibles.
        /// Para TipoVehiculo, las opciones son: Todos = 0, Pequenio = 1, Mediano = 2, Grande = 3, Camioneta = 4, Lujo = 5, Deportivo = 6.
        /// </summary>
        /// <param name="request">Criterios de busqueda.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(List<VehiculosDisponiblesResponse>), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> ListarVehiculosDisponibles([FromBody] ListarVehiculosDisponiblesRequest request)
        {
            var result = await sender.Send(request);

            return result.Match<IActionResult>(
                onSuccess: () => Ok(result.Value),
                onFailure: BadRequest);
        }

        /// <summary>
        /// Lista las localidades.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(List<ListarLocalidadesResponse>), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> ListarLocalidades()
        {
            var request = new ListarLocalidadesRequest();
            var result = await sender.Send(request);

            return result.Match<IActionResult>(
                onSuccess: () => Ok(result.Value),
                onFailure: BadRequest);
        }
    }
}