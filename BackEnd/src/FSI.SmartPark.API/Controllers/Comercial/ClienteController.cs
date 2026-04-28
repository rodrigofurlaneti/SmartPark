using FSI.SmartPark.Application.Commands.Comercial.Cliente;
using FSI.SmartPark.Application.Queries.Comercial.Cliente;
using FSI.SmartPark.Application.DTOs.Comercial;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Comercial;

/// <summary>
/// Controller REST — Cliente.
/// Padrão CQRS: leitura via Queries, escrita via Commands, tudo via ISender (MediatR).
/// </summary>
[ApiController]
[Route("api/clientes")]
[Produces("application/json")]
public class ClienteController : ControllerBase
{
    private readonly ISender _sender;
    public ClienteController(ISender sender) => _sender = sender;

    // ── Queries ──────────────────────────────────────────────────────────────

    /// <summary>Lista todos os registros ativos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClienteResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _sender.Send(new GetAllClientesQuery(), ct));

    /// <summary>Busca por Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClienteResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetClienteByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    // ── Commands ─────────────────────────────────────────────────────────────

    /// <summary>Cria novo Cliente.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ClienteResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateClienteRequest create, CancellationToken ct)
    {
        var result = await _sender.Send(
            new CreateClienteCommand(create.Nome,
            create.Documento,
            create.IsMensalista,
            create.EmpresaId), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Atualiza Cliente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ClienteResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClienteRequest update, CancellationToken ct)
    {
        var result = await _sender.Send(
            new UpdateClienteCommand(id,
            update.Nome,
            update.IsMensalista), ct);
        return Ok(result);
    }
    /// <summary>Soft Delete — marca IsDeleted = true.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteClienteCommand(id), ct);
        return NoContent();
    }
}

/// <summary>Payload para criação de Cliente.</summary>
public sealed record CreateClienteRequest(
    string Nome,
    string Documento,
    bool IsMensalista,
    int EmpresaId
);

/// <summary>Payload para atualização de Cliente.</summary>
public sealed record UpdateClienteRequest(
    string Nome,
    bool IsMensalista
);
