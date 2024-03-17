using Bdv.Reservas.Aplicacion.Dto.Command;
using Fabrela.FabrelaResult.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bdv.Reservas.Api.Command.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ReservasController(ISender sender) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(void), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> CrearReserva([FromBody] CrearReservaRequest request)
        {
            var result = await sender.Send(request);

            return result.Match<IActionResult>(
                onSuccess: () => Ok(result),
                onFailure: BadRequest);
        }
    }
}