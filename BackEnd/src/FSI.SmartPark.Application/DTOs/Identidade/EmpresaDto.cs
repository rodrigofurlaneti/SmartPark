using FSI.SmartPark.Domain.Entities.Identidade;

namespace FSI.SmartPark.Application.DTOs.Identidade;

/// <summary>DTO de resposta para Empresa — retornado pelas Queries e Commands.</summary>
public record EmpresaResponseDto(
    int      Id,
    string   NomeFantasia,
    string   Cnpj,
    string?  RazaoSocial,
    string?  Email,
    string?  Telefone,
    bool     Ativa,
    DateTime DataInsercao
)
{
    /// <summary>
    /// Factory method que converte a entidade de domínio para DTO.
    /// Mantém a lógica de mapeamento centralizada no próprio DTO (sem AutoMapper).
    /// </summary>
    public static EmpresaResponseDto FromEntity(Empresa e) => new(
        e.Id,
        e.NomeFantasia,
        e.Cnpj,
        e.RazaoSocial,
        e.Email,
        e.Telefone,
        e.Ativa,
        e.DataInsercao);
}
