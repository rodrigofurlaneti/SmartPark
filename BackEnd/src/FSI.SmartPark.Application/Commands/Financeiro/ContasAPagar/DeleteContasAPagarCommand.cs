using MediatR;

namespace FSI.SmartPark.Application.Commands.Financeiro.ContasAPagar;

/// <summary>Soft Delete de ContasAPagar (IsDeleted = true).</summary>
public sealed record DeleteContasAPagarCommand(int Id) : IRequest<bool>;
