using FSI.SmartPark.Application.DTOs.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Suporte.Usuario;

/// <summary>Lista todos os registros ativos de Usuario.</summary>
public sealed record GetAllUsuariosQuery() : IRequest<IEnumerable<UsuarioResponseDto>>;
