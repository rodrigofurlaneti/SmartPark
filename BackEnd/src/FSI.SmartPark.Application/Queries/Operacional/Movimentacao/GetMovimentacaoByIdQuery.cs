using FSI.SmartPark.Application.DTOs.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Operacional.Movimentacao;

/// <summary>Busca Movimentacao por Id.</summary>
public sealed record GetMovimentacaoByIdQuery(int Id) : IRequest<MovimentacaoResponseDto?>;
