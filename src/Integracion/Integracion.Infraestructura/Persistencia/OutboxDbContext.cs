using Integracion.Core.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Integracion.Infraestructura.Persistencia
{
    public class OutboxDbContext : DbContext
    {
        public OutboxDbContext(DbContextOptions<OutboxDbContext> options) : base(options)
        {
        }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }
    }
}