using FSI.SmartPark.Domain.Enums;
namespace FSI.SmartPark.Application.DTOs.Equipe;

public record RegistrarPontoRequestDto(
    int FuncionarioId,
    TipoRegistroPonto TipoRegistro,
    int? UnidadeId = null
);

public record ControlePontoResponseDto(
    int Id,
    int FuncionarioId,
    DateTime DataRegistro,
    TipoRegistroPonto TipoRegistro,
    int? UnidadeId
);
