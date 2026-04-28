using FSI.SmartPark.Application.DTOs.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Suporte.Usuario;

public sealed class CreateUsuarioCommandHandler
    : IRequestHandler<CreateUsuarioCommand, UsuarioResponseDto>
{
    private readonly IUsuarioRepository _repo;
    public CreateUsuarioCommandHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<UsuarioResponseDto> Handle(CreateUsuarioCommand request, CancellationToken ct)
    {
        var hash = System.Convert.ToHexString(
            System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(request.Senha))).ToLower();
        var entidade = new FSI.SmartPark.Domain.Entities.Suporte.Usuario(request.Login, hash, request.EmpresaId, request.UnidadeId);
        var id = await _repo.Add(entidade, ct);
        var criado = await _repo.GetById(id, ct) ?? throw new InvalidOperationException("Usuario não encontrado após inserção.");
        return ToDto(criado);
    }

    private static UsuarioResponseDto ToDto(FSI.SmartPark.Domain.Entities.Suporte.Usuario e) => new UsuarioResponseDto(e.Id, e.Login, e.Ativo, e.Unidade_Id, e.Funcionario_Id);
}
