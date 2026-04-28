using FSI.SmartPark.Application.DTOs.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Operacional.Faturamento;

/// <summary>Lista todos os registros ativos de Faturamento.</summary>
public sealed record GetAllFaturamentosQuery() : IRequest<IEnumerable<FaturamentoResponseDto>>;
