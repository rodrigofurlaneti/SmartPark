using MediatR;

namespace FSI.SmartPark.Application.Commands.Suporte.Estoque;

/// <summary>Soft Delete de Estoque (IsDeleted = true).</summary>
public sealed record DeleteEstoqueCommand(int Id) : IRequest<bool>;
