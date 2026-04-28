using FSI.SmartPark.Application.DTOs.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Operacional.Faturamento;

/// <summary>Busca Faturamento por Id.</summary>
public sealed record GetFaturamentoByIdQuery(int Id) : IRequest<FaturamentoResponseDto?>;
