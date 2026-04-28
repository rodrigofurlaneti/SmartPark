using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Application.Interfaces.Operacional;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Operacional;

[ApiController]
[Route("api/[controller]")]
public class FaturamentoController : ControllerBase
{
    private readonly IFaturamentoService _service;

    public FaturamentoController(IFaturamentoService service) => _service = service;

    [HttpPost("abrir-turno")]
    public async Task<IActionResult> AbrirTurno([FromBody] AbrirTurnoRequestDto dto)
    {
        var result = await _service.AbrirTurno(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
    }

    [HttpPut("fechar-turno")]
    public async Task<IActionResult> FecharTurno([FromBody] FecharTurnoRequestDto dto)
        => Ok(await _service.FecharTurno(dto));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var result = await _service.ObterPorId(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("periodo/{unidadeId:int}")]
    public async Task<IActionResult> ListarPorPeriodo(
        int unidadeId, [FromQuery] DateTime inicio, [FromQuery] DateTime fim)
        => Ok(await _service.ListarPorPeriodo(unidadeId, inicio, fim));

    [HttpPost("{id:int}/sangria/{valor:decimal}")]
    public async Task<IActionResult> Sangria(int id, decimal valor)
    {
        await _service.RegistrarSangria(id, valor);
        return NoContent();
    }
}
