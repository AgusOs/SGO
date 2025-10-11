using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGO.Domain.Turnos;

namespace SGO.Infrastructure.Persistence.Configurations;

public class TurnoConfig : IEntityTypeConfiguration<Turno>
{
    public void Configure(EntityTypeBuilder<Turno> builder)
    {
        builder.ToTable("turnos");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.FechaHora)
               .IsRequired()
               .HasColumnType("datetime");

        builder.Property(t => t.Estado)
               .HasConversion<string>()
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(t => t.Motivo).HasMaxLength(500);

        builder.HasOne(t => t.Paciente)
               .WithMany(p => p.Turnos) // navegación de solo lectura
               .HasForeignKey(t => t.PacienteDocumento)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
