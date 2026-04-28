namespace FSI.SmartPark.Application.DTOs.Comercial;

public record ConvenioRequestDto(
    string Descricao,
    bool Ativo
);

public record ConvenioResponseDto(
    int Id,
    string Descricao,
    bool Ativo,
    DateTime DataInsercao
);
