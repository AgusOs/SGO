using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGO.Domain.Fichas;
using SGO.Domain.Odontogramas;
using SGO.Domain.Procedimientos;

namespace SGO.Infrastructure.Persistence.Configurations;

public class FichaClinicaConfig : IEntityTypeConfiguration<FichaClinica>
{
    public void Configure(EntityTypeBuilder<FichaClinica> builder)
    {
        builder.ToTable("fichas_clinicas");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.MotivoConsulta)
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(f => f.Diagnostico).HasMaxLength(1000);
        builder.Property(f => f.Prescripciones).HasMaxLength(1000);
        builder.Property(f => f.Observaciones).HasMaxLength(1000);
        builder.Property(f => f.FechaCreacionUtc)
               .HasColumnType("datetime")
               .IsRequired();

        builder.HasOne(f => f.Paciente)
               .WithMany()
               .HasForeignKey(f => f.PacienteDocumento)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Odontograma)
               .WithOne()
               .HasForeignKey<Odontograma>(o => o.FichaClinicaId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(f => f.Procedimientos)
               .WithOne()
               .HasForeignKey("FichaClinicaId")
               .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(f => f.Procedimientos)
               .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(f => f.PacienteDocumento);
        builder.HasIndex(f => f.ProfesionalMatricula);
        builder.HasIndex(f => f.TurnoId);
    }
}
