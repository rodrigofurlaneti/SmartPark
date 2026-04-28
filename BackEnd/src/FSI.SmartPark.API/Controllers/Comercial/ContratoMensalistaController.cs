using FSI.SmartPark.Application.Commands.Comercial.ContratoMensalista;
using FSI.SmartPark.Application.Queries.Comercial.ContratoMensalista;
using FSI.SmartPark.Application.DTOs.Comercial;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Comercial;

/// <summary>
/// Controller REST — ContratoMensalista.
/// Padrão CQRS: leitura via Queries, escrita via Commands, tudo via ISender (MediatR).
/// </summary>
[ApiController]
[Route("api/contratos-mensalistas")]
[Produces("application/json")]
public class ContratoMensalistaController : ControllerBase
{
    private readonly ISender _sender;
    public ContratoMensalistaController(ISender sender) => _sender = sender;

    // ── Queries ──────────────────────────────────────────────────────────────

    /// <summary>Lista todos os registros ativos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ContratoMensalistaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _sender.Send(new GetAllContratosMensalistasQuery(), ct));

    /// <summary>Busca por Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ContratoMensalistaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetContratoMensalistaByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    // ── Commands ─────────────────────────────────────────────────────────────

    /// <summary>Cria novo ContratoMensalista.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ContratoMensalistaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateContratoMensalistaRequest create, CancellationToken ct)
    {
        var result = await _sender.Send(
            new CreateContratoMensalistaCommand(create.ClienteId,
            create.UnidadeId,
            create.Valor,
            create.EmpresaId), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Atualiza ContratoMensalista.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ContratoMensalistaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateContratoMensalistaRequest update, CancellationToken ct)
    {
        var result = await _sender.Send(
            new UpdateContratoMensalistaCommand(id,
            update.Valor), ct);
        return Ok(result);
    }
    /// <summary>Soft Delete — marca IsDeleted = true.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteContratoMensalistaCommand(id), ct);
        return NoContent();
    }
}

/// <summary>Payload para criação de ContratoMensalista.</summary>
public sealed record CreateContratoMensalistaRequest(
    int ClienteId,
    int UnidadeId,
    decimal Valor,
    int EmpresaId
);

/// <summary>Payload para atualização de ContratoMensalista.</summary>
public sealed record UpdateContratoMensalistaRequest(
    decimal Valor
);
