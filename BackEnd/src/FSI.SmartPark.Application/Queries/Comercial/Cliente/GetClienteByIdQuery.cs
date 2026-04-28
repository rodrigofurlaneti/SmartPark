using FSI.SmartPark.Application.DTOs.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Comercial.Cliente;

/// <summary>Busca Cliente por Id.</summary>
public sealed record GetClienteByIdQuery(int Id) : IRequest<ClienteResponseDto?>;
