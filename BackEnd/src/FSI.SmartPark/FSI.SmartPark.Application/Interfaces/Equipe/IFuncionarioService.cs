using FSI.SmartPark.Application.DTOs.Equipe;
namespace FSI.SmartPark.Application.Interfaces.Equipe;

public interface IFuncionarioService
{
    Task<FuncionarioResponseDto> Criar(FuncionarioRequestDto dto);
    Task<FuncionarioResponseDto?> ObterPorId(int id);
    Task<IEnumerable<FuncionarioResponseDto>> ListarAtivos();
    Task<IEnumerable<FuncionarioResponseDto>> ListarPorUnidade(int unidadeId);
    Task AlterarSalario(int id, decimal novoSalario);
    Task Desligar(int id);
}
