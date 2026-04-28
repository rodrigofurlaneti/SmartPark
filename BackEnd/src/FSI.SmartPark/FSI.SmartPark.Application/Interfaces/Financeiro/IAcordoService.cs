using FSI.SmartPark.Application.DTOs.Financeiro;
namespace FSI.SmartPark.Application.Interfaces.Financeiro;

public interface IAcordoService
{
    Task<AcordoResponseDto> Criar(AcordoRequestDto dto);
    Task<AcordoResponseDto?> ObterPorId(int id);
    Task<IEnumerable<AcordoResponseDto>> ListarPorCliente(int clienteId);
    Task PagarParcela(int parcelaId);
}
