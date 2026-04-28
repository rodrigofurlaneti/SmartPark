using FSI.SmartPark.Application.DTOs.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Suporte.Usuario;

/// <summary>Command CQRS para atualizar Usuario.</summary>
public sealed record UpdateUsuarioCommand(
    int Id,
    bool Ativo
) : IRequest<UsuarioResponseDto>;
