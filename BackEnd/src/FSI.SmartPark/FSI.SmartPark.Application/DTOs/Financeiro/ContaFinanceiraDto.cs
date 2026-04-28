namespace FSI.SmartPark.Application.DTOs.Financeiro;

public record ContaFinanceiraRequestDto(
    string Descricao,
    string Agencia,
    string Conta,
    int EmpresaId,
    int? BancoId = null,
    string? Carteira = null,
    string? Convenio = null,
    string? CodigoTransmissao = null
);

public record ContaFinanceiraResponseDto(
    int Id,
    string Descricao,
    string Agencia,
    string Conta,
    bool ContaPadrao,
    int? BancoId,
    int EmpresaId,
    bool PodeGerarBoleto
);
