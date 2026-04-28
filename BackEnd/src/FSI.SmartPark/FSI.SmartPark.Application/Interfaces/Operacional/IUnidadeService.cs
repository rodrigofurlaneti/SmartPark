using FSI.SmartPark.Application.DTOs.Operacional;
namespace FSI.SmartPark.Application.Interfaces.Operacional;

public interface IUnidadeService
{
    Task<UnidadeResponseDto> Criar(UnidadeRequestDto dto);
    Task<UnidadeResponseDto> Atualizar(int id, UnidadeRequestDto dto);
    Task<UnidadeResponseDto?> ObterPorId(int id);
    Task<IEnumerable<UnidadeResponseDto>> ListarAtivas();
    Task<IEnumerable<UnidadeResponseDto>> ListarTodas();
    Task Inativar(int id);
}
