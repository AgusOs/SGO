using Microsoft.EntityFrameworkCore;
using SGO.Application.Interfaces;
using SGO.Domain.Pacientes;
using SGO.Infrastructure.Persistence.Context;

namespace SGO.Infrastructure.Persistence.Repositories;

public sealed class PacienteRepository : IPacienteRepository
{
    private readonly SGOContext _context;

    public PacienteRepository(SGOContext context)
    {
        _context = context;
    }

    public async Task<Paciente?> GetByDocumentoAsync(int documento, bool includeTurnos = false)
    {
        IQueryable<Paciente> query = _context.Pacientes;

        if (includeTurnos)
            query = query.Include(p => p.Turnos);

        return await query.FirstOrDefaultAsync(p => p.Documento == documento);
    }

    public async Task<List<Paciente>> GetAllAsync()
        => await _context.Pacientes.AsNoTracking().ToListAsync();

    public async Task AddAsync(Paciente paciente)
        => await _context.Pacientes.AddAsync(paciente);

    public Task UpdateAsync(Paciente paciente)
    {
        _context.Pacientes.Update(paciente);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int documento)
        => await _context.Pacientes.AnyAsync(p => p.Documento == documento);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
