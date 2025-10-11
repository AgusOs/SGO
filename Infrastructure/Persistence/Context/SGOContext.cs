using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SGO.Domain.Fichas;
using SGO.Domain.Odontogramas;
using SGO.Domain.Pacientes;
using SGO.Domain.Procedimientos;
using SGO.Domain.Turnos;

namespace SGO.Infrastructure.Persistence.Context;

/// <summary>
/// Contexto principal de la aplicación SGO.
/// Encapsula el acceso a la base de datos MySQL mediante Entity Framework Core.
/// </summary>
public class SGOContext : DbContext
{
    // --- Constructor ---
    public SGOContext(DbContextOptions<SGOContext> options)
        : base(options) { }

    // --- DbSets (Tablas Principales) ---
    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<FichaClinica> FichasClinicas => Set<FichaClinica>();
    public DbSet<Odontograma> Odontogramas => Set<Odontograma>();
    public DbSet<User> Usuarios => Set<User>();
    public DbSet<Procedimiento> Procedimientos => Set<Procedimiento>();
    public DbSet<Turno> Turnos => Set<Turno>();

    // --- Configuración global ---
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica automáticamente todas las configuraciones de IEntityTypeConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SGOContext).Assembly);

        // Opcional: convención global de nombres en minúscula
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName()?.ToLowerInvariant());
        }

        base.OnModelCreating(modelBuilder);
    }

    // --- Opcional: configuración de auditoría automática ---
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            // Si tenés campos como FechaCreacion / FechaModificacion, podés setearlos aquí
            if (entry.Properties.Any(p => p.Metadata.Name == "FechaCreacionUtc") &&
                entry.State == EntityState.Added)
            {
                entry.Property("FechaCreacionUtc").CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
