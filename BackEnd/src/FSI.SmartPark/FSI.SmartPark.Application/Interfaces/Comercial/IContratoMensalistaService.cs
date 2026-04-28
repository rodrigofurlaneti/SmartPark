using FSI.SmartPark.Application.DTOs.Comercial;
namespace FSI.SmartPark.Application.Interfaces.Comercial;

public interface IContratoMensalistaService
{
    Task<ContratoMensalistaResponseDto> Criar(ContratoMensalistaRequestDto dto);
    Task<ContratoMensalistaResponseDto?> ObterPorId(int id);
    Task<IEnumerable<ContratoMensalistaResponseDto>> ListarPorCliente(int clienteId);
    Task<IEnumerable<ContratoMensalistaResponseDto>> ListarPorUnidade(int unidadeId);
    Task Renovar(int id);
    Task Bloquear(int id);
}
