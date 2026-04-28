using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Faturamento;

/// <summary>Soft Delete de Faturamento (IsDeleted = true).</summary>
public sealed record DeleteFaturamentoCommand(int Id) : IRequest<bool>;
