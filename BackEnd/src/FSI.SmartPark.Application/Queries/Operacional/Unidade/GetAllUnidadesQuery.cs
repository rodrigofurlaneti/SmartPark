using FSI.SmartPark.Application.DTOs.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Operacional.Unidade;

/// <summary>Lista todos os registros ativos de Unidade.</summary>
public sealed record GetAllUnidadesQuery() : IRequest<IEnumerable<UnidadeResponseDto>>;
