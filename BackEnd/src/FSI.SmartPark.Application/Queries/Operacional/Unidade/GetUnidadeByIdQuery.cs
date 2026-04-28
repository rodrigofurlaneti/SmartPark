using FSI.SmartPark.Application.DTOs.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Operacional.Unidade;

/// <summary>Busca Unidade por Id.</summary>
public sealed record GetUnidadeByIdQuery(int Id) : IRequest<UnidadeResponseDto?>;
