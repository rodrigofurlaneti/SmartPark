using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Application.Interfaces.Operacional;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Operacional;

[ApiController]
[Route("api/[controller]")]
public class MovimentacaoController : ControllerBase
{
    private readonly IMovimentacaoService _service;

    public MovimentacaoController(IMovimentacaoService service) => _service = service;

    /// <summary>Registra entrada de veículo.</summary>
    [HttpPost("entrada")]
    public async Task<IActionResult> Entrada([FromBody] EntradaVeiculoRequestDto dto)
    {
        var result = await _service.RegistrarEntrada(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
    }

    /// <summary>Registra saída e calcula cobrança.</summary>
    [HttpPost("saida")]
    public async Task<IActionResult> Saida([FromBody] SaidaVeiculoRequestDto dto)
    {
        var result = await _service.RegistrarSaida(dto);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var result = await _service.ObterPorId(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Lista movimentações em aberto de uma unidade.</summary>
    [HttpGet("abertas/{unidadeId:int}")]
    public async Task<IActionResult> ListarAbertas(int unidadeId)
        => Ok(await _service.ListarAbertas(unidadeId));

    /// <summary>Histórico por período.</summary>
    [HttpGet("periodo/{unidadeId:int}")]
    public async Task<IActionResult> ListarPorPeriodo(
        int unidadeId,
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fim)
        => Ok(await _service.ListarPorPeriodo(unidadeId, inicio, fim));
}
