using FSI.SmartPark.Application.DTOs.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Comercial.Cliente;

/// <summary>Lista todos os registros ativos de Cliente.</summary>
public sealed record GetAllClientesQuery() : IRequest<IEnumerable<ClienteResponseDto>>;
