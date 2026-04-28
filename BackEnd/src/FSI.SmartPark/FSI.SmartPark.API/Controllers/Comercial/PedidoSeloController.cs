using FSI.SmartPark.Application.DTOs.Comercial;
using FSI.SmartPark.Application.Interfaces.Comercial;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Comercial;

[ApiController]
[Route("api/[controller]")]
public class PedidoSeloController : ControllerBase
{
    private readonly IPedidoSeloService _service;

    public PedidoSeloController(IPedidoSeloService service) => _service = service;

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var result = await _service.ObterPorId(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("cliente/{clienteId:int}")]
    public async Task<IActionResult> ListarPorCliente(int clienteId)
        => Ok(await _service.ListarPorCliente(clienteId));

    [HttpGet("pendentes/{unidadeId:int}")]
    public async Task<IActionResult> ListarPendentes(int unidadeId)
        => Ok(await _service.ListarPendentes(unidadeId));

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] PedidoSeloRequestDto dto)
    {
        var result = await _service.Criar(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Cancelar(int id)
    {
        await _service.Cancelar(id);
        return NoContent();
    }
}
