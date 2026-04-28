namespace FSI.SmartPark.Application.DTOs.Comercial;

public record ContratoMensalistaRequestDto(
    int ClienteId,
    int UnidadeId,
    decimal Valor,
    int NumeroVagas,
    string HorarioInicio,
    string HorarioFim
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
