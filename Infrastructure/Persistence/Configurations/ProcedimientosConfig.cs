using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGO.Domain.Procedimientos;

namespace SGO.Infrastructure.Persistence.Configurations;

public class ProcedimientoConfig : IEntityTypeConfiguration<Procedimiento>
{
    public void Configure(EntityTypeBuilder<Procedimiento> builder)
    {
        builder.ToTable("procedimientos");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Tipo).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Descripcion).HasMaxLength(500);
        builder.Property(p => p.Fecha).IsRequired();
    }
}