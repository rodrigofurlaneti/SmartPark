using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Movimentacao;

/// <summary>Soft Delete de Movimentacao (IsDeleted = true).</summary>
public sealed record DeleteMovimentacaoCommand(int Id) : IRequest<bool>;
