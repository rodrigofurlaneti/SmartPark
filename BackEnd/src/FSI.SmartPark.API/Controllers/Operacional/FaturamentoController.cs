using FSI.SmartPark.Application.Commands.Operacional.Faturamento;
using FSI.SmartPark.Application.Queries.Operacional.Faturamento;
using FSI.SmartPark.Application.DTOs.Operacional;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Operacional;

/// <summary>
/// Controller REST — Faturamento.
/// Padrão CQRS: leitura via Queries, escrita via Commands, tudo via ISender (MediatR).
/// </summary>
[ApiController]
[Route("api/faturamentos")]
[Produces("application/json")]
public class FaturamentoController : ControllerBase
{
    private readonly ISender _sender;
    public FaturamentoController(ISender sender) => _sender = sender;

    // ── Queries ──────────────────────────────────────────────────────────────

    /// <summary>Lista todos os registros ativos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FaturamentoResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _sender.Send(new GetAllFaturamentosQuery(), ct));

    /// <summary>Busca por Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FaturamentoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetFaturamentoByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    // ── Commands ─────────────────────────────────────────────────────────────

    /// <summary>Cria novo Faturamento.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(FaturamentoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateFaturamentoRequest create, CancellationToken ct)
    {
        var result = await _sender.Send(
            new CreateFaturamentoCommand(create.NumFechamento,
            create.NumTerminal,
            create.UnidadeId,
            create.UsuarioId,
            create.EmpresaId,
            create.SaldoInicial), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Atualiza Faturamento.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(FaturamentoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFaturamentoRequest update, CancellationToken ct)
    {
        var result = await _sender.Send(
            new UpdateFaturamentoCommand(id,
            update.ValorTotal,
            update.ValorDinheiro,
            update.ValorCartaoDebito,
            update.ValorCartaoCredito,
            update.ValorRotativo,
            update.ValorMensalidade,
            update.ValorSemParar,
            update.ValorSeloDesconto,
            update.TicketFinal), ct);
        return Ok(result);
    }
    /// <summary>Soft Delete — marca IsDeleted = true.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteFaturamentoCommand(id), ct);
        return NoContent();
    }
}

/// <summary>Payload para criação de Faturamento.</summary>
public sealed record CreateFaturamentoRequest(
    int NumFechamento,
    int NumTerminal,
    int UnidadeId,
    int UsuarioId,
    int EmpresaId,
    decimal SaldoInicial
);

/// <summary>Payload para atualização de Faturamento.</summary>
public sealed record UpdateFaturamentoRequest(
    decimal ValorTotal,
    decimal ValorDinheiro,
    decimal ValorCartaoDebito,
    decimal ValorCartaoCredito,
    decimal? ValorRotativo,
    decimal? ValorMensalidade,
    decimal? ValorSemParar,
    decimal? ValorSeloDesconto,
    string? TicketFinal
);
