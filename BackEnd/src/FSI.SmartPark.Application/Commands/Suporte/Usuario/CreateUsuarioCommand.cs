using FSI.SmartPark.Application.DTOs.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Suporte.Usuario;

/// <summary>Command CQRS para criar Usuario.</summary>
public sealed record CreateUsuarioCommand(
    string Login,
    string Senha,
    int EmpresaId,
    int? UnidadeId
) : IRequest<UsuarioResponseDto>;
