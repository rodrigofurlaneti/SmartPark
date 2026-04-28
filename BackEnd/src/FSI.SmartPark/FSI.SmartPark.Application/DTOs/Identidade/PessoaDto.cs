namespace FSI.SmartPark.Application.DTOs.Identidade;

public record PessoaRequestDto(
    string Nome,
    string? Sexo = null,
    DateTime? DataNascimento = null
);

public record PessoaResponseDto(
    int Id,
    string Nome,
    string? Sexo,
    DateTime? DataNascimento,
    bool Ativo,
    DateTime DataInsercao
);
