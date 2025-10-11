using Microsoft.EntityFrameworkCore;
using SGO.Application.Interfaces;
using SGO.Domain.Turnos;
using SGO.Infrastructure.Persistence.Context;

namespace SGO.Infrastructure.Persistence.Repositories;

public sealed class TurnoRepository : ITurnoRepository
{
    private readonly SGOContext _context;

    public TurnoRepository(SGOContext context)
    {
        _context = context;
    }

    public async Task<Turno?> GetByIdAsync(Guid id)
        => await _context.Turnos.Include(t => t.Paciente).FirstOrDefaultAsync(t => t.Id == id);

    public async Task<List<Turno>> GetByPacienteAsync(int documentoPaciente)
        => await _context.Turnos
            .Where(t => t.PacienteDocumento == documentoPaciente)
            .OrderByDescending(t => t.FechaHora)
            .ToListAsync();

    public async Task<List<Turno>> GetByFechaAsync(DateOnly fecha)
    {
        var desde = fecha.ToDateTime(TimeOnly.MinValue);
        var hasta = fecha.ToDateTime(TimeOnly.MaxValue);

        return await _context.Turnos
            .Where(t => t.FechaHora >= desde && t.FechaHora <= hasta)
            .Include(t => t.Paciente)
            .OrderBy(t => t.FechaHora)
            .ToListAsync();
    }

    public async Task AddAsync(Turno turno)
        => await _context.Turnos.AddAsync(turno);

    public Task UpdateAsync(Turno turno)
    {
        _context.Turnos.Update(turno);
        return Task.CompletedTask;
    }

    public async Task<bool> ExisteTurnoEnHorarioAsync(int profesionalMatricula, DateTime fechaHora)
    {
        return await _context.Turnos.AnyAsync(t =>
            t.ProfesionalMatricula == profesionalMatricula &&
            t.FechaHora == fechaHora &&
            t.Estado != EstadoTurno.Cancelado);
    }

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
