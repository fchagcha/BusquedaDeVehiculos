using Bdv.Reservas.Api.Query.Controllers;
using Bdv.Reservas.Aplicacion.Dto.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Bdv.Reservas.Api.Query.Test
{
    public class ListarVehiculosDisponiblesTest
    {
        [Test]
        public async Task TestListarVehiculosDisponibles()
        {
            var idLocalidadRecogida = Guid.NewGuid();
            var idLocalidadDevolucion = Guid.NewGuid();

            var mediatorMock = new Mock<ISender>();

            // Configura el objeto simulado para devolver un resultado válido
            var expectedResult = new List<VehiculosDisponiblesResponse>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ListarVehiculosDisponiblesRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var controller = new ReservasController(mediatorMock.Object);
            var request = new ListarVehiculosDisponiblesRequest(
                idLocalidadRecogida,
                idLocalidadDevolucion,
                FechaDeRecogida: new DateTime(2024, 03, 17),
                FechaDeDevolucion: new DateTime(2024, 03, 17),
                TipoVehiculo: 0);

            var actionResult = await controller.ListarVehiculosDisponibles(request);
            var okResult = actionResult as OkObjectResult;
            Assert.IsNotNull(okResult);

            var response = okResult.Value as List<VehiculosDisponiblesResponse>;

            mediatorMock.Verify(m => m.Send(request, It.IsAny<CancellationToken>()), Times.Once);

            // Verifica que la respuesta es el resultado esperado
            Assert.AreEqual(expectedResult, response);
        }
    }
}