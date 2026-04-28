namespace FSI.SmartPark.Application.DTOs.Financeiro;

public record AcordoRequestDto(
    string Descricao,
    decimal ValorTotal,
    int ClienteId,
    int? UnidadeId = null,
    int NumeroParcelas = 1
);

public record AcordoResponseDto(
    int Id,
    string Descricao,
    decimal ValorTotal,
    int? ClienteId,
    int? UnidadeId,
    List<AcordoParcelaResponseDto> Parcelas
);

public record AcordoParcelaResponseDto(
    int Id,
    int Numero,
    DateTime DataVencimento,
    decimal Valor,
    bool Pago
);
