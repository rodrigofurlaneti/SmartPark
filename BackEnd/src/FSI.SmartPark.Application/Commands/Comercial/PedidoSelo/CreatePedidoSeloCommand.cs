using FSI.SmartPark.Application.DTOs.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.PedidoSelo;

/// <summary>Command CQRS para criar PedidoSelo.</summary>
public sealed record CreatePedidoSeloCommand(
    int Quantidade,
    int ClienteId,
    int UnidadeId,
    int TipoSeloId,
    FSI.SmartPark.Domain.Enums.FormaPagamento FormaPagamento,
    FSI.SmartPark.Domain.Enums.TipoPedidoSelo TipoPedido,
    int UsuarioId,
    int DiasVencimento,
    int? ConvenioId
) : IRequest<PedidoSeloResponseDto>;
