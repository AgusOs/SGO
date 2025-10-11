using SGO.Application.Interfaces;
using SGO.Domain.Fichas;

namespace SGO.Application.UseCases.Fichas.Queries;

public class ObtenerByIdCommand
{
    private readonly IFichaClinicaRepository _fichas;

    public ObtenerByIdCommand(IFichaClinicaRepository fichas)
    {
        _fichas = fichas;
    }

    public async Task<FichaClinica?> EjecutarAsync(Guid id)
    {
        return await _fichas.GetByIdAsync(id);
    }
}
