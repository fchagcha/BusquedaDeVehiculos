using Integracion.Comun.Events;
using Integracion.Core.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Integracion.Core.Outbox
{
    [Table("OutboxMessage")]
    public class OutboxMessage
    {
        public OutboxMessage() { }
        public OutboxMessage(IIntegrationEvent evento)
        {

            Id = Guid.NewGuid();
            IntegrationEventType = evento.GetType().FullName;
            TypeRequest = evento.Content.GetType().FullName;
            JsonRequest = JsonConvert.SerializeObject(evento.Content);
            Status = Status.Pendiente;
            CreatedAt = DateTime.Now;
        }

        [Key]
        public Guid Id { get; set; }
        public string IntegrationEventType { get; set; }
        public string TypeRequest { get; set; }
        public string JsonRequest { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
    }
}