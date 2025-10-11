using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGO.Domain.Procedimientos;

public class ProcedimientoConfig : IEntityTypeConfiguration<Procedimiento>
{
    public void Configure(EntityTypeBuilder<Procedimiento> builder)
    {
        builder.ToTable("procedimientos");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Profesional)
               .HasMaxLength(150)
               .IsRequired();

        builder.Property(p => p.Estado)
               .HasConversion<string>()
               .HasMaxLength(20)
               .IsRequired();

        builder.HasDiscriminator<string>("Discriminator")
               //.HasValue<Obturacion>("Obturacion")
               .HasValue<Extraccion>("Extraccion")
               .HasValue<Limpieza>("Limpieza");
               //.HasValue<Endodoncia>("Endodoncia");
    }
}
