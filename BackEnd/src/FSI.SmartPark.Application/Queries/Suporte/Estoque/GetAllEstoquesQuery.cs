using FSI.SmartPark.Application.DTOs.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Suporte.Estoque;

/// <summary>Lista todos os registros ativos de Estoque.</summary>
public sealed record GetAllEstoquesQuery() : IRequest<IEnumerable<EstoqueResponseDto>>;
