using FSI.SmartPark.Application.Commands.Operacional.Movimentacao;
using FSI.SmartPark.Application.Queries.Operacional.Movimentacao;
using FSI.SmartPark.Application.DTOs.Operacional;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Operacional;

/// <summary>
/// Controller REST — Movimentacao.
/// Padrão CQRS: leitura via Queries, escrita via Commands, tudo via ISender (MediatR).
/// </summary>
[ApiController]
[Route("api/movimentacoes")]
[Produces("application/json")]
public class MovimentacaoController : ControllerBase
{
    private readonly ISender _sender;
    public MovimentacaoController(ISender sender) => _sender = sender;

    // ── Queries ──────────────────────────────────────────────────────────────

    /// <summary>Lista todos os registros ativos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MovimentacaoResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _sender.Send(new GetAllMovimentacoesQuery(), ct));

    /// <summary>Busca por Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MovimentacaoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetMovimentacaoByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    // ── Commands ─────────────────────────────────────────────────────────────

    /// <summary>Cria novo Movimentacao.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(MovimentacaoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMovimentacaoRequest create, CancellationToken ct)
    {
        var result = await _sender.Send(
            new CreateMovimentacaoCommand(create.Placa,
            create.UnidadeId,
            create.EmpresaId,
            create.ClienteId,
            create.NumeroContrato), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Atualiza Movimentacao.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(MovimentacaoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMovimentacaoRequest update, CancellationToken ct)
    {
        var result = await _sender.Send(
            new UpdateMovimentacaoCommand(id,
            update.ValorCobrado,
            update.FormaPagamento,
            update.CpfParaNF), ct);
        return Ok(result);
    }
    /// <summary>Soft Delete — marca IsDeleted = true.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteMovimentacaoCommand(id), ct);
        return NoContent();
    }
}

/// <summary>Payload para criação de Movimentacao.</summary>
public sealed record CreateMovimentacaoRequest(
    string Placa,
    int UnidadeId,
    int EmpresaId,
    int? ClienteId,
    string? NumeroContrato
);

/// <summary>Payload para atualização de Movimentacao.</summary>
public sealed record UpdateMovimentacaoRequest(
    decimal ValorCobrado,
    string? FormaPagamento,
    string? CpfParaNF
);
