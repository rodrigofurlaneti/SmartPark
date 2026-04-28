using FSI.SmartPark.Domain.Enums;
namespace FSI.SmartPark.Application.DTOs.Operacional;

public record VeiculoRequestDto(
    string Placa,
    TipoVeiculo TipoVeiculo,
    int? ModeloId = null
);

public record VeiculoResponseDto(
    int Id,
    string Placa,
    TipoVeiculo TipoVeiculo,
    string? Cor,
    int? Ano,
    int? ModeloId
);
