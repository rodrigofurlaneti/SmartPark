using MediatR;

namespace FSI.SmartPark.Application.Commands.Equipe.ControlePonto;

/// <summary>Soft Delete de ControlePonto (IsDeleted = true).</summary>
public sealed record DeleteControlePontoCommand(int Id) : IRequest<bool>;
