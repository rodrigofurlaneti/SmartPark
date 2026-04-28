using FSI.SmartPark.Application.Commands.Comercial.PedidoSelo;
using FSI.SmartPark.Application.Queries.Comercial.PedidoSelo;
using FSI.SmartPark.Application.DTOs.Comercial;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Comercial;

/// <summary>
/// Controller REST — PedidoSelo.
/// Padrão CQRS: leitura via Queries, escrita via Commands, tudo via ISender (MediatR).
/// </summary>
[ApiController]
[Route("api/pedidos-selo")]
[Produces("application/json")]
public class PedidoSeloController : ControllerBase
{
    private readonly ISender _sender;
    public PedidoSeloController(ISender sender) => _sender = sender;

    // ── Queries ──────────────────────────────────────────────────────────────

    /// <summary>Lista todos os registros ativos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PedidoSeloResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _sender.Send(new GetAllPedidoSelosQuery(), ct));

    /// <summary>Busca por Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PedidoSeloResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetPedidoSeloByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    // ── Commands ─────────────────────────────────────────────────────────────

    /// <summary>Cria novo PedidoSelo.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PedidoSeloResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePedidoSeloRequest create, CancellationToken ct)
    {
        var result = await _sender.Send(
            new CreatePedidoSeloCommand(create.Quantidade,
            create.ClienteId,
            create.UnidadeId,
            create.TipoSeloId,
            create.FormaPagamento,
            create.TipoPedido,
            create.UsuarioId,
            create.DiasVencimento,
            create.ConvenioId), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Atualiza PedidoSelo.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PedidoSeloResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePedidoSeloRequest update, CancellationToken ct)
    {
        var result = await _sender.Send(
            new UpdatePedidoSeloCommand(id,
            update.Status), ct);
        return Ok(result);
    }
    /// <summary>Soft Delete — marca IsDeleted = true.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeletePedidoSeloCommand(id), ct);
        return NoContent();
    }
}

/// <summary>Payload para criação de PedidoSelo.</summary>
public sealed record CreatePedidoSeloRequest(
    int Quantidade,
    int ClienteId,
    int UnidadeId,
    int TipoSeloId,
    FSI.SmartPark.Domain.Enums.FormaPagamento FormaPagamento,
    FSI.SmartPark.Domain.Enums.TipoPedidoSelo TipoPedido,
    int UsuarioId,
    int DiasVencimento,
    int? ConvenioId
);

/// <summary>Payload para atualização de PedidoSelo.</summary>
public sealed record UpdatePedidoSeloRequest(
    FSI.SmartPark.Domain.Enums.StatusPedidoSelo Status
);
