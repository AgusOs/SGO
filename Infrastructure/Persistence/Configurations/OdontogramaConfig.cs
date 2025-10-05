using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGO.Domain.Odontogramas;

namespace SGO.Infrastructure.Persistence.Configurations;

public class OdontogramaConfig : IEntityTypeConfiguration<Odontograma>
{
    public void Configure(EntityTypeBuilder<Odontograma> builder)
    {
        builder.ToTable("odontogramas");

        // Shared Primary Key con FichaClinica
        builder.HasKey(o => o.FichaClinicaId);
        builder.Property(o => o.FichaClinicaId).ValueGeneratedNever();

        // Owned collection de piezas
        builder.OwnsMany(o => o.Piezas, piezas =>
        {
            piezas.ToTable("odontograma_piezas");

            piezas.WithOwner().HasForeignKey("OdontogramaId");

            piezas.Property(p => p.Fdi).IsRequired();

            piezas.Property(p => p.Estado)
                  .HasConversion<string>()
                  .HasMaxLength(50);

            piezas.Property(p => p.M).HasConversion<string>().HasMaxLength(50);
            piezas.Property(p => p.D).HasConversion<string>().HasMaxLength(50);
            piezas.Property(p => p.V).HasConversion<string>().HasMaxLength(50);
            piezas.Property(p => p.L).HasConversion<string>().HasMaxLength(50);
            piezas.Property(p => p.O).HasConversion<string>().HasMaxLength(50);
        });
    }
}
