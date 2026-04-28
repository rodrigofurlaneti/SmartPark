using FSI.SmartPark.Application.DTOs.Equipe;
using FSI.SmartPark.Application.Interfaces.Equipe;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Equipe;

[ApiController]
[Route("api/[controller]")]
public class ControlePontoController : ControllerBase
{
    private readonly IControlePontoService _service;

    public ControlePontoController(IControlePontoService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> RegistrarPonto([FromBody] RegistrarPontoRequestDto dto)
        => Ok(await _service.RegistrarPonto(dto));

    [HttpGet("funcionario/{funcionarioId:int}")]
    public async Task<IActionResult> ListarPorFuncionario(
        int funcionarioId, [FromQuery] DateTime? data = null)
        => Ok(await _service.ListarPorFuncionario(funcionarioId, data ?? DateTime.Today));
}
