using FSI.SmartPark.Application.DTOs.Equipe;
namespace FSI.SmartPark.Application.Interfaces.Equipe;

public interface IControlePontoService
{
    Task<ControlePontoResponseDto> RegistrarPonto(RegistrarPontoRequestDto dto);
    Task<IEnumerable<ControlePontoResponseDto>> ListarPorFuncionario(int funcionarioId, DateTime data);
}
