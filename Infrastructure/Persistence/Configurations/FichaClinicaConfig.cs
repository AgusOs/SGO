using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGO.Domain.Fichas;
using SGO.Domain.Odontogramas;

namespace SGO.Infrastructure.Persistence.Configurations;

public class FichaClinicaConfig : IEntityTypeConfiguration<FichaClinica>
{
    public void Configure(EntityTypeBuilder<FichaClinica> builder)
    {
        builder.ToTable("fichas_clinicas");

        // --- Clave primaria ---
        builder.HasKey(f => f.Id);

        // --- Propiedades principales ---
        builder.Property(f => f.Id)
               .ValueGeneratedNever();

        builder.Property(f => f.PacienteDocumento)
               .IsRequired();

        builder.Property(f => f.ProfesionalMatricula)
               .IsRequired();

        builder.Property(f => f.TurnoId)
               .IsRequired();

        builder.Property(f => f.MotivoConsulta)
               .HasMaxLength(250)
               .IsRequired();

        builder.Property(f => f.Diagnostico)
               .HasMaxLength(1000);

        builder.Property(f => f.TratamientosRealizados)
               .HasMaxLength(2000);

        builder.Property(f => f.Prescripciones)
               .HasMaxLength(1000);

        builder.Property(f => f.Observaciones)
               .HasMaxLength(1000);

        builder.Property(f => f.FechaCreacionUtc)
               .HasColumnType("datetime")
               .IsRequired();

        // --- Relación N:1 con Paciente ---
        builder.HasOne(f => f.Paciente)
               .WithMany(p => p.FichasClinicas)
               .HasForeignKey(f => f.PacienteDocumento)
               .OnDelete(DeleteBehavior.Cascade);

        // --- Relación 1:1 con Odontograma ---
        builder.HasOne(f => f.Odontograma)
               .WithOne()
               .HasForeignKey<Odontograma>(o => o.FichaClinicaId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
