namespace FSI.SmartPark.Application.DTOs.Comercial;

public record ClienteRequestDto(
    string Nome,
    string Documento,
    bool IsMensalista
);

public record ClienteResponseDto(
    int Id,
    string Nome,
    string DocumentoNumero,
    bool IsMensalista,
    bool Ativo,
    DateTime DataInsercao
);
