using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webinar.API.Infrastructure.EntityConfigurations;

public class WebinarEntityTypeConfiguration : IEntityTypeConfiguration<Entities.Webinar>
{
    public void Configure(EntityTypeBuilder<Entities.Webinar> builder)
    {
        builder.ToTable("Webinar");

        builder.Property(ci => ci.Id)
            .UseHiLo("webinar_hilo")
            .IsRequired();

        builder.Property(ci => ci.Name)
            .IsRequired(true)
            .HasMaxLength(50);
    }
}