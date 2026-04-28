using FSI.SmartPark.Application.DTOs.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Suporte.Usuario;

public sealed class UpdateUsuarioCommandHandler
    : IRequestHandler<UpdateUsuarioCommand, UsuarioResponseDto>
{
    private readonly IUsuarioRepository _repo;
    public UpdateUsuarioCommandHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<UsuarioResponseDto> Handle(UpdateUsuarioCommand request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct)
            ?? throw new KeyNotFoundException($"Usuario {request.Id} não encontrado.");
        entidade.Bloquear();
        await _repo.Update(entidade, ct);
        return ToDto(entidade);
    }

    private static UsuarioResponseDto ToDto(FSI.SmartPark.Domain.Entities.Suporte.Usuario e) => new UsuarioResponseDto(e.Id, e.Login, e.Ativo, e.Unidade_Id, e.Funcionario_Id);
}
