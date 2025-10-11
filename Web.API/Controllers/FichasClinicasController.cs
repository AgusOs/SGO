using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGO.Application.UseCases.Fichas.Commands;
using SGO.Application.UseCases.Fichas.Queries;
using SGO.Domain.Odontogramas;
using SGO.Domain.Procedimientos;

namespace SGO.Web.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FichasClinicasController(
    RegistrarFichaClinicaCommand registrarFicha,
    ObtenerHistoriaCommand obtenerHistoria,
    ObtenerByIdCommand obtenerById
    ) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] RegistrarFichaClinicaDto dto)
    {
        var odontograma = Odontograma.CrearDesdePiezas(Guid.NewGuid(), dto.Piezas);

        var procedimientos = dto.Procedimientos.Select(p =>
            Procedimiento.Crear(p.Tipo, p.Descripcion, p.PiezaFdi, dto.ProfesionalMatricula))
            .ToList();

        var ficha = await registrarFicha.EjecutarAsync(
            dto.TurnoId,
            dto.MotivoConsulta,
            dto.Diagnostico,
            procedimientos,
            odontograma,
            dto.Prescripciones,
            dto.Observaciones
        );

        return CreatedAtAction(nameof(ObtenerPorId), new { id = ficha.Id }, ficha);
    }

    [HttpGet("{id:guid}")]
    public IActionResult ObtenerPorId(Guid id)
    {
        var ficha = obtenerById.EjecutarAsync(id);
        return Ok();
    }

    [HttpGet("paciente/{documento:int}")]
    public async Task<IActionResult> ObtenerPorPaciente(int documento)
    {
        var fichas = await obtenerHistoria.EjecutarAsync(documento);
        return Ok(fichas);
    }
}

public record RegistrarFichaClinicaDto(
    Guid TurnoId,
    string MotivoConsulta,
    string? Diagnostico,
    int ProfesionalMatricula,
    List<ProcedimientoDto> Procedimientos,
    List<PiezaDentalDto> Piezas,
    string? Prescripciones,
    string? Observaciones
);

public record ProcedimientoDto(string Tipo, string? Descripcion, int? PiezaFdi);