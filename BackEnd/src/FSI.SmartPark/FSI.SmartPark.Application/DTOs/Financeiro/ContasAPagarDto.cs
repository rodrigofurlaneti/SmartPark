using FSI.SmartPark.Domain.Enums;
namespace FSI.SmartPark.Application.DTOs.Financeiro;

public record ContasAPagarRequestDto(
    string NumeroDocumento,
    DateTime DataVencimento,
    decimal ValorTotal,
    int? FornecedorId = null,
    int? UnidadeId = null
);

public record ContasAPagarResponseDto(
    int Id,
    string NumeroDocumento,
    DateTime DataVencimento,
    decimal ValorTotal,
    StatusContasAPagar Status,
    int? FornecedorId,
    int? UnidadeId
);
