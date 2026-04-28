using FSI.SmartPark.Application.DTOs.Equipe;
using FSI.SmartPark.Application.Interfaces.Equipe;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Equipe;

[ApiController]
[Route("api/[controller]")]
public class FuncionarioController : ControllerBase
{
    private readonly IFuncionarioService _service;

    public FuncionarioController(IFuncionarioService service) => _service = service;

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var result = await _service.ObterPorId(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("ativos")]
    public async Task<IActionResult> ListarAtivos() => Ok(await _service.ListarAtivos());

    [HttpGet("unidade/{unidadeId:int}")]
    public async Task<IActionResult> ListarPorUnidade(int unidadeId)
        => Ok(await _service.ListarPorUnidade(unidadeId));

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] FuncionarioRequestDto dto)
    {
        var result = await _service.Criar(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}/salario/{valor:decimal}")]
    public async Task<IActionResult> AlterarSalario(int id, decimal valor)
    {
        await _service.AlterarSalario(id, valor);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Desligar(int id)
    {
        await _service.Desligar(id);
        return NoContent();
    }
}
