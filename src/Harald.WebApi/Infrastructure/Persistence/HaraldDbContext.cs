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

            modelBuilder.Entity<Capability>(cfg =>
            {
                cfg.ToTable("Capability");
                cfg.Property(e => e.SlackChannelId)
                    .HasConversion(v => v.ToString(), v => new ChannelId(v));
            });
        }
    }
}