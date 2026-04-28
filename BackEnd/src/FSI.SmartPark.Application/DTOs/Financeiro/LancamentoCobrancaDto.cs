using FSI.SmartPark.Domain.Enums;
namespace FSI.SmartPark.Application.DTOs.Financeiro;

public record LancamentoCobrancaResponseDto(
    int Id,
    int? ClienteId,
    DateTime DataVencimento,
    decimal ValorContrato,
    decimal ValorMulta,
    decimal ValorJuros,
    decimal TotalCalculado,
    StatusLancamentoCobranca Status
);
