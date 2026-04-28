using FSI.SmartPark.Application.DTOs.Suporte;
using FSI.SmartPark.Application.Interfaces.Suporte;
using FSI.SmartPark.Domain.Entities.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using System.Security.Cryptography;
using System.Text;

namespace FSI.SmartPark.Application.Services.Suporte;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repo;

    public UsuarioService(IUsuarioRepository repo) => _repo = repo;

    public async Task<UsuarioResponseDto> Criar(UsuarioRequestDto dto)
    {
        var senhaHash = HashSenha(dto.Senha);
        var usuario = new Usuario(dto.Login, senhaHash, dto.UnidadeId);
        var id = await _repo.Inserir(usuario);
        var criado = await _repo.ObterPorId(id);
        return ToDto(criado!);
    }

    public async Task<TokenResponseDto> Autenticar(UsuarioLoginDto dto)
    {
        // 1. Validar entrada
        if (string.IsNullOrWhiteSpace(dto.Login))
            throw new ArgumentException("Login é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.Senha))
            throw new ArgumentException("Senha é obrigatória.");

        // 2. Buscar usuários
        IEnumerable<Usuario> lista;
        try
        {
            lista = await _repo.ListarTodos();
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao acessar o banco de dados.", ex);
        }

        // 3. Verificar se retornou dados
        if (lista == null || !lista.Any())
            throw new Exception("Nenhum usuário cadastrado no sistema.");

        // 4. Buscar pelo login (sem senha ainda — para dar mensagens específicas)
        var usuarioPorLogin = lista.FirstOrDefault(u => u.Login == dto.Login);

        if (usuarioPorLogin == null)
            throw new UnauthorizedAccessException("Login não encontrado.");

        // 5. Verificar se está ativo
        if (!usuarioPorLogin.Ativo)
            throw new UnauthorizedAccessException("Usuário bloqueado. Contate o administrador.");

        // 6. Verificar senha
        var hashSenha = HashSenha(dto.Senha);
        if (usuarioPorLogin.Senha != hashSenha)
            throw new UnauthorizedAccessException("Senha incorreta.");

        // 7. Gerar token
        var token = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{usuarioPorLogin.Id}:{usuarioPorLogin.Login}:{DateTime.UtcNow:O}")
        );
        var expiracao = DateTime.UtcNow.AddHours(8);

        return new TokenResponseDto(token, expiracao, ToDto(usuarioPorLogin));
    }

    public async Task<UsuarioResponseDto?> ObterPorId(int id)
    {
        var u = await _repo.ObterPorId(id);
        return u is null ? null : ToDto(u);
    }

    public async Task Bloquear(int id)
    {
        var u = await _repo.ObterPorId(id)
            ?? throw new KeyNotFoundException($"Usuário {id} não encontrado.");
        u.Bloquear();
        await _repo.Atualizar(u);
    }

    private static string HashSenha(string senha)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(senha));
        return Convert.ToHexString(bytes).ToLower();
    }

    private static UsuarioResponseDto ToDto(Usuario u) => new(
        u.Id, u.Login, u.Ativo, u.Unidade_Id, u.Funcionario_Id);
}
