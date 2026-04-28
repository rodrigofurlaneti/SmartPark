using FSI.SmartPark.Application.DTOs.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Suporte.Usuario;

/// <summary>Busca Usuario por Id.</summary>
public sealed record GetUsuarioByIdQuery(int Id) : IRequest<UsuarioResponseDto?>;
