namespace FSI.SmartPark.Application.DTOs.Suporte;

public record EstoqueRequestDto(
    string Nome,
    int? UnidadeId = null,
    bool EstoquePrincipal = false
);

public record EstoqueResponseDto(
    int Id,
    string Nome,
    bool EstoquePrincipal,
    int? UnidadeId
);

public record EstoqueMaterialResponseDto(
    int EstoqueId,
    int MaterialId,
    string NomeMaterial,
    int Quantidade,
    int EstoqueMinimo,
    bool AbaixoDoMinimo
);
