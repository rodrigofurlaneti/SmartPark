using FSI.SmartPark.Application.Commands.Equipe.ControlePonto;
using FSI.SmartPark.Application.Queries.Equipe.ControlePonto;
using FSI.SmartPark.Application.DTOs.Equipe;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Equipe;

/// <summary>
/// Controller REST — ControlePonto.
/// Padrão CQRS: leitura via Queries, escrita via Commands, tudo via ISender (MediatR).
/// </summary>
[ApiController]
[Route("api/controle-ponto")]
[Produces("application/json")]
public class ControlePontoController : ControllerBase
{
    private readonly ISender _sender;
    public ControlePontoController(ISender sender) => _sender = sender;

    // ── Queries ──────────────────────────────────────────────────────────────

    /// <summary>Lista todos os registros ativos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ControlePontoResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _sender.Send(new GetAllControlePontosQuery(), ct));

    /// <summary>Busca por Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ControlePontoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetControlePontoByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    // ── Commands ─────────────────────────────────────────────────────────────

    /// <summary>Cria novo ControlePonto.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ControlePontoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateControlePontoRequest create, CancellationToken ct)
    {
        var result = await _sender.Send(
            new CreateControlePontoCommand(create.FuncionarioId,
            create.TipoRegistro,
            create.UnidadeId), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Soft Delete — marca IsDeleted = true.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteControlePontoCommand(id), ct);
        return NoContent();
    }
}

/// <summary>Payload para criação de ControlePonto.</summary>
public sealed record CreateControlePontoRequest(
    int FuncionarioId,
    FSI.SmartPark.Domain.Enums.TipoRegistroPonto TipoRegistro,
    int? UnidadeId
);


