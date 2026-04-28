using FSI.SmartPark.Application.DTOs.Operacional;
namespace FSI.SmartPark.Application.Interfaces.Operacional;

public interface IMovimentacaoService
{
    Task<MovimentacaoResponseDto> RegistrarEntrada(EntradaVeiculoRequestDto dto);
    Task<MovimentacaoResponseDto> RegistrarSaida(SaidaVeiculoRequestDto dto);
    Task<MovimentacaoResponseDto?> ObterPorId(int id);
    Task<IEnumerable<MovimentacaoResponseDto>> ListarAbertas(int unidadeId);
    Task<IEnumerable<MovimentacaoResponseDto>> ListarPorPeriodo(int unidadeId, DateTime inicio, DateTime fim);
}
