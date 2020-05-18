using Harald.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harald.WebApi.Infrastructure.Persistence
{
    public class CapabilityConfiguration : IEntityTypeConfiguration<Capability>
    {
        public void Configure(EntityTypeBuilder<Capability> builder)
        {
            builder.ToTable("Capability");
            builder.HasKey(cap => new { cap.Id, cap.SlackChannelId });
            builder.Property(e => e.SlackChannelId)
                    .HasConversion(v => v.ToString(), v => new ChannelId(v));

            builder.HasMany(b => b.Members).WithOne();

            var navigation = builder.Metadata.FindNavigation(nameof(Capability.Members));
            navigation?.SetField("_members");
            navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}