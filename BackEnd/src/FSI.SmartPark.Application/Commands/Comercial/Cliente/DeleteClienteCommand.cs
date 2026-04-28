using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.Cliente;

/// <summary>Soft Delete de Cliente (IsDeleted = true).</summary>
public sealed record DeleteClienteCommand(int Id) : IRequest<bool>;
