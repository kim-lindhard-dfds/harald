using Harald.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Harald.WebApi.Infrastructure.Persistence
{
    public class HaraldDbContext : DbContext
    {
        public HaraldDbContext(DbContextOptions<HaraldDbContext> options) : base(options)
        {
        }

        public DbSet<Capability> Capabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CapabilityConfiguration());
        }
    }
}