using FSI.SmartPark.Application.DTOs.Financeiro;
using FSI.SmartPark.Application.Interfaces.Financeiro;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Financeiro;

[ApiController]
[Route("api/[controller]")]
public class ContasAPagarController : ControllerBase
{
    private readonly IContasAPagarService _service;

    public ContasAPagarController(IContasAPagarService service) => _service = service;

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var result = await _service.ObterPorId(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("unidade/{unidadeId:int}")]
    public async Task<IActionResult> ListarPorUnidade(int unidadeId)
        => Ok(await _service.ListarPorUnidade(unidadeId));

    [HttpGet("em-aberto")]
    public async Task<IActionResult> ListarEmAberto()
        => Ok(await _service.ListarEmAberto());

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] ContasAPagarRequestDto dto)
    {
        var result = await _service.Criar(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}/pagar")]
    public async Task<IActionResult> Pagar(int id)
    {
        await _service.Pagar(id);
        return NoContent();
    }
}
