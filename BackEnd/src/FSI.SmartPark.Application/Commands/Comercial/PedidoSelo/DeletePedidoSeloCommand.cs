using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.PedidoSelo;

/// <summary>Soft Delete de PedidoSelo (IsDeleted = true).</summary>
public sealed record DeletePedidoSeloCommand(int Id) : IRequest<bool>;
