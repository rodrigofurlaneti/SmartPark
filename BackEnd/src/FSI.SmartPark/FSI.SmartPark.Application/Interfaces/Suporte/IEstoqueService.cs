using FSI.SmartPark.Application.DTOs.Suporte;
namespace FSI.SmartPark.Application.Interfaces.Suporte;

public interface IEstoqueService
{
    Task<EstoqueResponseDto> Criar(EstoqueRequestDto dto);
    Task<IEnumerable<EstoqueResponseDto>> ListarPorUnidade(int unidadeId);
    Task<IEnumerable<EstoqueMaterialResponseDto>> ListarMateriais(int estoqueId);
    Task AdicionarMaterial(int estoqueId, int materialId, int quantidade);
    Task RemoverMaterial(int estoqueId, int materialId, int quantidade);
}
