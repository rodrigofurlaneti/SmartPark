using FSI.SmartPark.Application.DTOs.Comercial;
using FSI.SmartPark.Application.Interfaces.Comercial;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Comercial;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly IClienteService _service;

    public ClienteController(IClienteService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> ListarTodos() => Ok(await _service.ListarTodos());

    [HttpGet("mensalistas")]
    public async Task<IActionResult> ListarMensalistas() => Ok(await _service.ListarMensalistas());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var result = await _service.ObterPorId(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("documento/{doc}")]
    public async Task<IActionResult> ObterPorDocumento(string doc)
    {
        var result = await _service.ObterPorDocumento(doc);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] ClienteRequestDto dto)
    {
        var result = await _service.Criar(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] ClienteRequestDto dto)
        => Ok(await _service.Atualizar(id, dto));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Inativar(int id)
    {
        await _service.Inativar(id);
        return NoContent();
    }
}
