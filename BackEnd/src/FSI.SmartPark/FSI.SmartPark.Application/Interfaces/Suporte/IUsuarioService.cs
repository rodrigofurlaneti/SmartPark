using FSI.SmartPark.Application.DTOs.Suporte;
namespace FSI.SmartPark.Application.Interfaces.Suporte;

public interface IUsuarioService
{
    Task<UsuarioResponseDto> Criar(UsuarioRequestDto dto);
    Task<TokenResponseDto> Autenticar(UsuarioLoginDto dto);
    Task<UsuarioResponseDto?> ObterPorId(int id);
    Task Bloquear(int id);
}
