using FSI.SmartPark.Domain.Enums;
namespace FSI.SmartPark.Application.DTOs.Equipe;

public record FuncionarioRequestDto(
    int PessoaId,
    decimal Salario,
    TipoEscalaFuncionario Escala,
    int? CargoId = null,
    int? UnidadeId = null
);

public record FuncionarioResponseDto(
    int Id,
    int PessoaId,
    string Codigo,
    decimal Salario,
    StatusFuncionario Status,
    TipoEscalaFuncionario TipoEscala,
    DateTime? DataAdmissao,
    int? CargoId,
    int? UnidadeId
);
