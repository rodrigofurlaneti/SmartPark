using FSI.SmartPark.Application.DTOs.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.PedidoSelo;

/// <summary>Command CQRS para atualizar PedidoSelo.</summary>
public sealed record UpdatePedidoSeloCommand(
    int Id,
    FSI.SmartPark.Domain.Enums.StatusPedidoSelo Status
) : IRequest<PedidoSeloResponseDto>;
