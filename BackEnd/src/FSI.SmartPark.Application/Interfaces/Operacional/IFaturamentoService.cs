using FSI.SmartPark.Application.DTOs.Operacional;
namespace FSI.SmartPark.Application.Interfaces.Operacional;

public interface IFaturamentoService
{
    Task<FaturamentoResponseDto> AbrirTurno(AbrirTurnoRequestDto dto);
    Task<FaturamentoResponseDto> FecharTurno(FecharTurnoRequestDto dto);
    Task<FaturamentoResponseDto?> ObterPorId(int id);
    Task<IEnumerable<FaturamentoResponseDto>> ListarPorPeriodo(int unidadeId, DateTime inicio, DateTime fim);
    Task RegistrarSangria(int faturamentoId, decimal valor);
}
