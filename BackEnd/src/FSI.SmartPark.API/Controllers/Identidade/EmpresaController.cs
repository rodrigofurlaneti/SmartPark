using FSI.SmartPark.Application.Commands.Empresa;
using FSI.SmartPark.Application.DTOs.Identidade;
using FSI.SmartPark.Application.Queries.Empresa;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Identidade;

/// <summary>
/// Controller REST para gerenciamento de Empresas (tenants) no SmartPark.
///
/// Padrão CQRS: todas as ações passam pelo ISender do MediatR.
///   - Leitura → Queries (sem efeito colateral)
///   - Escrita  → Commands (com efeito colateral)
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EmpresaController : ControllerBase
{
    private readonly ISender _sender;

    public EmpresaController(ISender sender) => _sender = sender;

    // ───────────────────────────────────────────────────────────────
    //  QUERIES — Leitura
    // ───────────────────────────────────────────────────────────────

    /// <summary>Lista todas as empresas ativas.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmpresaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _sender.Send(new GetAllEmpresasQuery(), ct));

    /// <summary>Busca uma empresa pelo Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EmpresaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetEmpresaByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    // ───────────────────────────────────────────────────────────────
    //  COMMANDS — Escrita
    // ───────────────────────────────────────────────────────────────

    /// <summary>Cria uma nova empresa (tenant).</summary>
    [HttpPost]
    [ProducesResponseType(typeof(EmpresaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateEmpresaCommand command,
        CancellationToken ct)
    {
        var result = await _sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Atualiza os dados cadastrais de uma empresa existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(EmpresaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateEmpresaRequest request,
        CancellationToken ct)
    {
        var command = new UpdateEmpresaCommand(
            id,
            request.NomeFantasia,
            request.RazaoSocial,
            request.Email,
            request.Telefone);

        var result = await _sender.Send(command, ct);
        return Ok(result);
    }

    /// <summary>
    /// Exclui logicamente uma empresa (Soft Delete).
    /// O registro permanece no banco com IsDeleted = true.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteEmpresaCommand(id), ct);
        return NoContent();
    }
}

/// <summary>DTO de entrada para a atualização da Empresa (separado do Command para não expor o Id na body).</summary>
public sealed record UpdateEmpresaRequest(
    string  NomeFantasia,
    string? RazaoSocial = null,
    string? Email       = null,
    string? Telefone    = null
);
