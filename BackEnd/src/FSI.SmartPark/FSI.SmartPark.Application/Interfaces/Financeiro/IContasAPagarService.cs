using FSI.SmartPark.Application.DTOs.Financeiro;
namespace FSI.SmartPark.Application.Interfaces.Financeiro;

public interface IContasAPagarService
{
    Task<ContasAPagarResponseDto> Criar(ContasAPagarRequestDto dto);
    Task<ContasAPagarResponseDto?> ObterPorId(int id);
    Task<IEnumerable<ContasAPagarResponseDto>> ListarPorUnidade(int unidadeId);
    Task<IEnumerable<ContasAPagarResponseDto>> ListarEmAberto();
    Task Pagar(int id);
}
