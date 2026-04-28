using FSI.SmartPark.Application.Commands.Financeiro.ContasAPagar;
using FSI.SmartPark.Application.Queries.Financeiro.ContasAPagar;
using FSI.SmartPark.Application.DTOs.Financeiro;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Financeiro;

/// <summary>
/// Controller REST — ContasAPagar.
/// Padrão CQRS: leitura via Queries, escrita via Commands, tudo via ISender (MediatR).
/// </summary>
[ApiController]
[Route("api/contas-a-pagar")]
[Produces("application/json")]
public class ContasAPagarController : ControllerBase
{
    private readonly ISender _sender;
    public ContasAPagarController(ISender sender) => _sender = sender;

    // ── Queries ──────────────────────────────────────────────────────────────

    /// <summary>Lista todos os registros ativos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ContasAPagarResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _sender.Send(new GetAllContasAPagarQuery(), ct));

    /// <summary>Busca por Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ContasAPagarResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetContasAPagarByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    // ── Commands ─────────────────────────────────────────────────────────────

    /// <summary>Cria novo ContasAPagar.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ContasAPagarResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateContasAPagarRequest create, CancellationToken ct)
    {
        var result = await _sender.Send(
            new CreateContasAPagarCommand(create.NumeroDocumento,
            create.DataVencimento,
            create.ValorTotal,
            create.EmpresaId,
            create.FornecedorId,
            create.UnidadeId), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Atualiza ContasAPagar.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ContasAPagarResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateContasAPagarRequest update, CancellationToken ct)
    {
        var result = await _sender.Send(
            new UpdateContasAPagarCommand(id,
            update.DataVencimento,
            update.ValorTotal), ct);
        return Ok(result);
    }
    /// <summary>Soft Delete — marca IsDeleted = true.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteContasAPagarCommand(id), ct);
        return NoContent();
    }
}

/// <summary>Payload para criação de ContasAPagar.</summary>
public sealed record CreateContasAPagarRequest(
    string NumeroDocumento,
    DateTime DataVencimento,
    decimal ValorTotal,
    int EmpresaId,
    int? FornecedorId,
    int? UnidadeId
);

/// <summary>Payload para atualização de ContasAPagar.</summary>
public sealed record UpdateContasAPagarRequest(
    DateTime DataVencimento,
    decimal ValorTotal
);
