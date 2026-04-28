using FSI.SmartPark.Domain.Enums;
namespace FSI.SmartPark.Application.DTOs.Comercial;

public record PedidoSeloRequestDto(
    int Quantidade,
    int ClienteId,
    int UnidadeId,
    int TipoSeloId,
    FormaPagamento FormaPagamento,
    TipoPedidoSelo TipoPedido,
    int UsuarioId,
    int DiasVencimento = 5,
    int? ConvenioId = null
);

public record PedidoSeloResponseDto(
    int Id,
    int Quantidade,
    StatusPedidoSelo Status,
    int ClienteId,
    int UnidadeId,
    int TipoSeloId,
    DateTime? DataVencimento,
    DateTime DataInsercao
);
