using FSI.SmartPark.Application.DTOs.Suporte;
using FSI.SmartPark.Application.Interfaces.Suporte;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Suporte;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _service;

    public UsuarioController(IUsuarioService service) => _service = service;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UsuarioLoginDto dto)
        => Ok(await _service.Autenticar(dto));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var result = await _service.ObterPorId(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] UsuarioRequestDto dto)
    {
        var result = await _service.Criar(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Bloquear(int id)
    {
        await _service.Bloquear(id);
        return NoContent();
    }
}
