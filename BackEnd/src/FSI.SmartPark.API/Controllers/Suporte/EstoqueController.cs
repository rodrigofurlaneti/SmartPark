using FSI.SmartPark.Application.Commands.Suporte.Estoque;
using FSI.SmartPark.Application.Queries.Suporte.Estoque;
using FSI.SmartPark.Application.DTOs.Suporte;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Suporte;

/// <summary>
/// Controller REST — Estoque.
/// Padrão CQRS: leitura via Queries, escrita via Commands, tudo via ISender (MediatR).
/// </summary>
[ApiController]
[Route("api/estoques")]
[Produces("application/json")]
public class EstoqueController : ControllerBase
{
    private readonly ISender _sender;
    public EstoqueController(ISender sender) => _sender = sender;

    // ── Queries ──────────────────────────────────────────────────────────────

    /// <summary>Lista todos os registros ativos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EstoqueResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _sender.Send(new GetAllEstoquesQuery(), ct));

    /// <summary>Busca por Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EstoqueResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetEstoqueByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    // ── Commands ─────────────────────────────────────────────────────────────

    /// <summary>Cria novo Estoque.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(EstoqueResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateEstoqueRequest create, CancellationToken ct)
    {
        var result = await _sender.Send(
            new CreateEstoqueCommand(create.Nome,
            create.EmpresaId,
            create.UnidadeId,
            create.EstoquePrincipal), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Atualiza Estoque.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(EstoqueResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEstoqueRequest update, CancellationToken ct)
    {
        var result = await _sender.Send(
            new UpdateEstoqueCommand(id,
            update.Nome,
            update.EstoquePrincipal), ct);
        return Ok(result);
    }
    /// <summary>Soft Delete — marca IsDeleted = true.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteEstoqueCommand(id), ct);
        return NoContent();
    }
}

/// <summary>Payload para criação de Estoque.</summary>
public sealed record CreateEstoqueRequest(
    string Nome,
    int EmpresaId,
    int? UnidadeId,
    bool EstoquePrincipal
);

/// <summary>Payload para atualização de Estoque.</summary>
public sealed record UpdateEstoqueRequest(
    string Nome,
    bool EstoquePrincipal
);
