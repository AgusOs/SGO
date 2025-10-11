using Microsoft.EntityFrameworkCore;
using SGO.Application.Interfaces;
using SGO.Domain.Fichas;
using SGO.Infrastructure.Persistence.Context;

namespace SGO.Infrastructure.Persistence.Repositories;

public sealed class FichaClinicaRepository : IFichaClinicaRepository
{
    private readonly SGOContext _context;

    public FichaClinicaRepository(SGOContext context)
    {
        _context = context;
    }

    public async Task<FichaClinica?> GetByIdAsync(Guid id)
        => await _context.FichasClinicas
            .Include(f => f.Procedimientos)
            .Include(f => f.Odontograma)
            .FirstOrDefaultAsync(f => f.Id == id);

    public async Task AddAsync(FichaClinica ficha)
        => await _context.FichasClinicas.AddAsync(ficha);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public async Task<List<FichaClinica>> GetByPacienteAsync(int documentoPaciente)
    {
        return await _context.FichasClinicas
            .Include(f => f.Odontograma)
            .Include(f => f.Procedimientos)
            .Where(f => f.PacienteDocumento == documentoPaciente)
            .OrderByDescending(f => f.FechaCreacionUtc)
            .ToListAsync();
    }
}
