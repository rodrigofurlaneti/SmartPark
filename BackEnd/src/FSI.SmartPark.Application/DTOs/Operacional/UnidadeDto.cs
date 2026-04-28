namespace FSI.SmartPark.Application.DTOs.Operacional;

public record UnidadeRequestDto(
    string Nome,
    int NumeroVagas,
    int DiaVencimento,
    int EmpresaId,
    string? CNPJ = null,
    string? CCM = null
);

public record UnidadeResponseDto(
    int Id,
    string Codigo,
    string Nome,
    int NumeroVaga,
    bool Ativa,
    int DiaVencimento,
    string? CNPJ,
    int? EmpresaId,
    int? FuncionarioId,
    int? EnderecoId
);
