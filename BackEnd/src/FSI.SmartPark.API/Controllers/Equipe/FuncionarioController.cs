using FSI.SmartPark.Application.Commands.Equipe.Funcionario;
using FSI.SmartPark.Application.Queries.Equipe.Funcionario;
using FSI.SmartPark.Application.DTOs.Equipe;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Equipe;

/// <summary>
/// Controller REST — Funcionario.
/// Padrão CQRS: leitura via Queries, escrita via Commands, tudo via ISender (MediatR).
/// </summary>
[ApiController]
[Route("api/funcionarios")]
[Produces("application/json")]
public class FuncionarioController : ControllerBase
{
    private readonly ISender _sender;
    public FuncionarioController(ISender sender) => _sender = sender;

    // ── Queries ──────────────────────────────────────────────────────────────

    /// <summary>Lista todos os registros ativos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FuncionarioResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _sender.Send(new GetAllFuncionariosQuery(), ct));

    /// <summary>Busca por Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FuncionarioResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetFuncionarioByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    // ── Commands ─────────────────────────────────────────────────────────────

    /// <summary>Cria novo Funcionario.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(FuncionarioResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateFuncionarioRequest create, CancellationToken ct)
    {
        var result = await _sender.Send(
            new CreateFuncionarioCommand(create.PessoaId,
            create.Salario,
            create.Escala,
            create.EmpresaId), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Atualiza Funcionario.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(FuncionarioResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFuncionarioRequest update, CancellationToken ct)
    {
        var result = await _sender.Send(
            new UpdateFuncionarioCommand(id,
            update.Salario), ct);
        return Ok(result);
    }
    /// <summary>Soft Delete — marca IsDeleted = true.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteFuncionarioCommand(id), ct);
        return NoContent();
    }
}

/// <summary>Payload para criação de Funcionario.</summary>
public sealed record CreateFuncionarioRequest(
    int PessoaId,
    decimal Salario,
    FSI.SmartPark.Domain.Enums.TipoEscalaFuncionario Escala,
    int EmpresaId
);

/// <summary>Payload para atualização de Funcionario.</summary>
public sealed record UpdateFuncionarioRequest(
    decimal Salario
);
