using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Unidade;

/// <summary>Soft Delete de Unidade (IsDeleted = true).</summary>
public sealed record DeleteUnidadeCommand(int Id) : IRequest<bool>;
