using FSI.SmartPark.Application.DTOs.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Operacional.Movimentacao;

/// <summary>Lista todos os registros ativos de Movimentacao.</summary>
public sealed record GetAllMovimentacoesQuery() : IRequest<IEnumerable<MovimentacaoResponseDto>>;
