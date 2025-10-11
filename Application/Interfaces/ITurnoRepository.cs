using SGO.Domain.Turnos;

namespace SGO.Application.Interfaces;

public interface ITurnoRepository
{
    Task<Turno?> GetByIdAsync(Guid id);
    Task<List<Turno>> GetByPacienteAsync(int documentoPaciente);
    Task<List<Turno>> GetByFechaAsync(DateOnly fecha);
    Task AddAsync(Turno turno);
    Task UpdateAsync(Turno turno);
    Task SaveChangesAsync();
    Task<bool> ExisteTurnoEnHorarioAsync(int profesionalMatricula, DateTime fechaHora);
}
