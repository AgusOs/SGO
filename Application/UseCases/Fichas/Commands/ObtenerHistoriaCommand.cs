using SGO.Domain.Fichas;
using SGO.Application.Interfaces;

namespace SGO.Application.UseCases.Fichas.Queries;

public class ObtenerHistoriaCommand
{
    private readonly IFichaClinicaRepository _fichas;

    public ObtenerHistoriaCommand(IFichaClinicaRepository fichas)
    {
        _fichas = fichas;
    }

    public async Task<List<FichaClinica>> EjecutarAsync(int documentoPaciente)
    {
        return await _fichas.GetByPacienteAsync(documentoPaciente);
    }
}
