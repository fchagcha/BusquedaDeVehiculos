using Bdv.Cqrs.Interfaces;
using Exceptionless;
using Fabrela.FabrelaResult.Abstractions;
using Integracion.Dto.Commands;

namespace Bdv.EnvioCorreo.Aplicacion
{
    public class EnvioCorreoHandler : ICommandHandler<EnviarCorreoReservaCommand>
    {
        public async Task<Result> Handle(EnviarCorreoReservaCommand request, CancellationToken cancellationToken)
        {
            ExceptionlessClient
                .Default
                .CreateLog($"Envio de correo Conductor: {request.NombreConductor}")
                .Submit();

            await Task.Yield();
            return Result.Success();
        }
    }
}