using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Application.Interfaces.Operacional;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Operacional;

[ApiController]
[Route("api/[controller]")]
public class UnidadeController : ControllerBase
{
    private readonly IUnidadeService _service;

    public UnidadeController(IUnidadeService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> ListarTodas() => Ok(await _service.ListarTodas());

    [HttpGet("ativas")]
    public async Task<IActionResult> ListarAtivas() => Ok(await _service.ListarAtivas());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var result = await _service.ObterPorId(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] UnidadeRequestDto dto)
    {
        var result = await _service.Criar(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] UnidadeRequestDto dto)
        => Ok(await _service.Atualizar(id, dto));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Inativar(int id)
    {
        await _service.Inativar(id);
        return NoContent();
    }
}
