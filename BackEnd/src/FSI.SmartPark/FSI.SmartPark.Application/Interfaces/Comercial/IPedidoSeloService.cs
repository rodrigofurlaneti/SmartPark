using FSI.SmartPark.Application.DTOs.Comercial;
namespace FSI.SmartPark.Application.Interfaces.Comercial;

public interface IPedidoSeloService
{
    Task<PedidoSeloResponseDto> Criar(PedidoSeloRequestDto dto);
    Task<PedidoSeloResponseDto?> ObterPorId(int id);
    Task<IEnumerable<PedidoSeloResponseDto>> ListarPorCliente(int clienteId);
    Task<IEnumerable<PedidoSeloResponseDto>> ListarPendentes(int unidadeId);
    Task Cancelar(int id);
}
