using FSI.SmartPark.Application.Commands.Operacional.Unidade;
using FSI.SmartPark.Application.Queries.Operacional.Unidade;
using FSI.SmartPark.Application.DTOs.Operacional;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Operacional;

/// <summary>
/// Controller REST — Unidade.
/// Padrão CQRS: leitura via Queries, escrita via Commands, tudo via ISender (MediatR).
/// </summary>
[ApiController]
[Route("api/unidades")]
[Produces("application/json")]
public class UnidadeController : ControllerBase
{
    private readonly ISender _sender;
    public UnidadeController(ISender sender) => _sender = sender;

    // ── Queries ──────────────────────────────────────────────────────────────

    /// <summary>Lista todos os registros ativos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UnidadeResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _sender.Send(new GetAllUnidadesQuery(), ct));

    /// <summary>Busca por Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UnidadeResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetUnidadeByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    // ── Commands ─────────────────────────────────────────────────────────────

    /// <summary>Cria novo Unidade.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(UnidadeResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUnidadeRequest create, CancellationToken ct)
    {
        var result = await _sender.Send(
            new CreateUnidadeCommand(create.Nome,
            create.NumeroVagas,
            create.DiaVencimento,
            create.EmpresaId,
            create.CNPJ,
            create.CCM), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Atualiza Unidade.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(UnidadeResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUnidadeRequest update, CancellationToken ct)
    {
        var result = await _sender.Send(
            new UpdateUnidadeCommand(id,
            update.Nome,
            update.NumeroVagas,
            update.DiaVencimento), ct);
        return Ok(result);
    }
    /// <summary>Soft Delete — marca IsDeleted = true.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteUnidadeCommand(id), ct);
        return NoContent();
    }
}

/// <summary>Payload para criação de Unidade.</summary>
public sealed record CreateUnidadeRequest(
    string Nome,
    int NumeroVagas,
    int DiaVencimento,
    int EmpresaId,
    string? CNPJ,
    string? CCM
);

/// <summary>Payload para atualização de Unidade.</summary>
public sealed record UpdateUnidadeRequest(
    string Nome,
    int NumeroVagas,
    int DiaVencimento
);
