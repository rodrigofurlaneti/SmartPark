using FSI.SmartPark.Application.Commands.Suporte.Usuario;
using FSI.SmartPark.Application.Queries.Suporte.Usuario;
using FSI.SmartPark.Application.DTOs.Suporte;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FSI.SmartPark.API.Controllers.Suporte;

/// <summary>
/// Controller REST — Usuario.
/// Padrão CQRS: leitura via Queries, escrita via Commands, tudo via ISender (MediatR).
/// </summary>
[ApiController]
[Route("api/usuarios")]
[Produces("application/json")]
public class UsuarioController : ControllerBase
{
    private readonly ISender _sender;
    public UsuarioController(ISender sender) => _sender = sender;

    // ── Queries ──────────────────────────────────────────────────────────────

    /// <summary>Lista todos os registros ativos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _sender.Send(new GetAllUsuariosQuery(), ct));

    /// <summary>Busca por Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetUsuarioByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    // ── Commands ─────────────────────────────────────────────────────────────

    /// <summary>Cria novo Usuario.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUsuarioRequest create, CancellationToken ct)
    {
        var result = await _sender.Send(
            new CreateUsuarioCommand(create.Login,
            create.Senha,
            create.EmpresaId,
            create.UnidadeId), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Atualiza Usuario.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUsuarioRequest update, CancellationToken ct)
    {
        var result = await _sender.Send(
            new UpdateUsuarioCommand(id,
            update.Ativo), ct);
        return Ok(result);
    }
    /// <summary>Soft Delete — marca IsDeleted = true.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteUsuarioCommand(id), ct);
        return NoContent();
    }

    /// <summary>Autentica um usuário e retorna o token de acesso.</summary>
    [HttpPost("autenticar")]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Autenticar([FromBody] AutenticarRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(new AuthenticateCommand(request.Login, request.Senha), ct);
        return Ok(result);
    }
}

/// <summary>Payload para criação de Usuario.</summary>
public sealed record CreateUsuarioRequest(
    string Login,
    string Senha,
    int EmpresaId,
    int? UnidadeId
);

/// <summary>Payload para atualização de Usuario.</summary>
public sealed record UpdateUsuarioRequest(
    bool Ativo
);

/// <summary>Payload para autenticação de Usuario.</summary>
public sealed record AutenticarRequest(
    string Login,
    string Senha
);
