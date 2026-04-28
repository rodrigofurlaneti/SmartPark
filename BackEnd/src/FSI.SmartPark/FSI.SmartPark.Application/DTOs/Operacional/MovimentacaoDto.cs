namespace FSI.SmartPark.Application.DTOs.Operacional;

public record EntradaVeiculoRequestDto(
    string Placa,
    int UnidadeId,
    int? UsuarioId = null,
    int? ClienteId = null,
    string? NumeroContrato = null
);

public record SaidaVeiculoRequestDto(
    int MovimentacaoId,
    decimal ValorCobrado,
    string? FormaPagamento = null,
    string? CpfParaNF = null
);

public record MovimentacaoResponseDto(
    int Id,
    string Ticket,
    string Placa,
    DateTime DataEntrada,
    DateTime? DataSaida,
    decimal ValorCobrado,
    string? FormaPagamento,
    string? TipoCliente,
    int UnidadeId,
    bool EmAberto
);
