using System.Security.Cryptography;
using System.Text;
using FSI.SmartPark.Application.DTOs.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Suporte.Usuario;

public sealed class AuthenticateCommandHandler
    : IRequestHandler<AuthenticateCommand, TokenResponseDto>
{
    private readonly IUsuarioRepository _repo;
    public AuthenticateCommandHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<TokenResponseDto> Handle(AuthenticateCommand request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Login))
            throw new ArgumentException("Login é obrigatório.");
        if (string.IsNullOrWhiteSpace(request.Senha))
            throw new ArgumentException("Senha é obrigatória.");

        var lista = await _repo.GetAll(ct);

        var usuario = lista.FirstOrDefault(u => u.Login == request.Login)
            ?? throw new UnauthorizedAccessException("Login não encontrado.");

        if (!usuario.Ativo)
            throw new UnauthorizedAccessException("Usuário bloqueado. Contate o administrador.");

        if (usuario.Senha != HashSenha(request.Senha))
            throw new UnauthorizedAccessException("Senha incorreta.");

        var token = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{usuario.Id}:{usuario.Login}:{DateTime.UtcNow:O}"));
        var expiracao = DateTime.UtcNow.AddHours(8);

        var usuarioDto = new UsuarioResponseDto(
            usuario.Id, usuario.Login, usuario.Ativo,
            usuario.Unidade_Id, usuario.Funcionario_Id);

        return new TokenResponseDto(token, expiracao, usuarioDto);
    }

    private static string HashSenha(string senha) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(senha))).ToLower();
}
