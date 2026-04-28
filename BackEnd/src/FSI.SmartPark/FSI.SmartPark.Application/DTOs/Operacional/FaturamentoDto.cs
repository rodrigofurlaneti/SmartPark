namespace FSI.SmartPark.Application.DTOs.Operacional;

public record AbrirTurnoRequestDto(
    int NumFechamento,
    int NumTerminal,
    int UnidadeId,
    int UsuarioId,
    decimal SaldoInicial
);

public record FecharTurnoRequestDto(
    int FaturamentoId,
    decimal ValorTotal,
    decimal ValorDinheiro,
    decimal ValorCartaoDebito,
    decimal ValorCartaoCredito,
    decimal? ValorRotativo = null,
    decimal? ValorMensalidade = null,
    decimal? ValorSemParar = null,
    decimal? ValorSeloDesconto = null,
    string? TicketFinal = null
);

public record FaturamentoResponseDto(
    int Id,
    int NumFechamento,
    int NumTerminal,
    int UnidadeId,
    DateTime DataAbertura,
    DateTime DataFechamento,
    decimal ValorTotal,
    decimal ValorDinheiro,
    decimal ValorCartaoDebito,
    decimal ValorCartaoCredito,
    decimal? ValorRotativo,
    decimal? ValorSemParar,
    decimal? SaldoInicial,
    decimal? ValorSangria,
    int? UsuarioId
);
