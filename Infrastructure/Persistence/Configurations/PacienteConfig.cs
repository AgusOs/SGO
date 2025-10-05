using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGO.Domain.Pacientes;

namespace SGO.Infrastructure.Persistence.Configurations;

public class PacienteConfig : IEntityTypeConfiguration<Paciente>
{
    public void Configure(EntityTypeBuilder<Paciente> builder)
    {
        builder.ToTable("pacientes");

        builder.HasKey(p => p.Documento);
        builder.Property(p => p.Documento)
               .ValueGeneratedNever(); // No autoincremental, lo define el usuario

        builder.Property(p => p.Nombre).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Apellido).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Email).HasMaxLength(150);
        builder.Property(p => p.Telefono).HasMaxLength(50);
        builder.Property(p => p.ObraSocial).HasMaxLength(100);
        builder.Property(p => p.NumeroAfiliado).HasMaxLength(50);
        builder.Property(p => p.Observaciones).HasMaxLength(1000);

        builder.HasMany(p => p.FichasClinicas)
               .WithOne(f => f.Paciente)
               .HasForeignKey(f => f.PacienteDocumento);
    }
}
