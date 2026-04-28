using FSI.SmartPark.Domain.Enums;
namespace FSI.SmartPark.Application.DTOs.Operacional;

public record TabelaPrecoAvulsoRequestDto(
    string Nome,
    decimal ValorHoraAdicional,
    int UsuarioId,
    int TempoToleranciaDesistencia = 10,
    int TempoToleranciaPagamento = 15,
    string? HoraInicioVigencia = null,
    string? HoraFimVigencia = null
);

public record TabelaPrecoAvulsoResponseDto(
    int Id,
    string Nome,
    decimal ValorHoraAdicional,
    bool Padrao,
    StatusSolicitacao Status,
    string? HoraInicioVigencia,
    string? HoraFimVigencia
);
