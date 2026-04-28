namespace FSI.SmartPark.Application.DTOs.Comercial;

public record ContratoMensalistaRequestDto(
    int     ClienteId,
    int     UnidadeId,
    decimal Valor,
    int     EmpresaId,
    int     NumeroVagas  = 1,
    string? HorarioInicio = null,
    string? HorarioFim    = null
);

public record ContratoMensalistaResponseDto(
    int Id,
    int NumeroContrato,
    int ClienteId,
    int UnidadeId,
    decimal Valor,
    bool Ativo,
    DateTime DataVencimento,
    int NumeroVagas
);
