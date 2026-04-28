namespace FSI.SmartPark.Application.DTOs.Identidade;

public record EmpresaRequestDto(
    int PessoaJuridicaId,
    int? GrupoId = null
);

public record EmpresaResponseDto(
    int Id,
    int PessoaJuridicaId,
    int? GrupoId,
    DateTime DataInsercao
);
