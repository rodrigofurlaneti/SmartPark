using FSI.SmartPark.Application.DTOs.Comercial;
namespace FSI.SmartPark.Application.Interfaces.Comercial;

public interface IClienteService
{
    Task<ClienteResponseDto> Criar(ClienteRequestDto dto);
    Task<ClienteResponseDto> Atualizar(int id, ClienteRequestDto dto);
    Task<ClienteResponseDto?> ObterPorId(int id);
    Task<ClienteResponseDto?> ObterPorDocumento(string documento);
    Task<IEnumerable<ClienteResponseDto>> ListarMensalistas();
    Task<IEnumerable<ClienteResponseDto>> ListarTodos();
    Task Inativar(int id);
}
