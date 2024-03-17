using Bdv.Cqrs.Interfaces;
using Integracion.Comun.Events;

namespace Integracion.Dto.Extensions
{
    public static class IntegrationEventExtensions
    {
        public static TEvent ToIntegrationEvent<TEvent>(this ICommand command) where TEvent : IIntegrationEvent, new()
        {
            return new TEvent
            {
                Content = command
            };
        }
    }
}
