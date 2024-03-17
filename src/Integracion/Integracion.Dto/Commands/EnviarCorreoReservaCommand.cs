using Bdv.Cqrs.Interfaces;

namespace Integracion.Dto.Commands
{
    public class EnviarCorreoReservaCommand : ICommand
    {
        public Guid Id { get; set; }
        public string NombreConductor { get; private set; }
        public string CorreoElectronicoConductor { get; private set; }
    }
}