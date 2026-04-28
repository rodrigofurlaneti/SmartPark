using FSI.SmartPark.Domain.Interfaces.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Suporte.Usuario;

public sealed class DeleteUsuarioCommandHandler
    : IRequestHandler<DeleteUsuarioCommand, bool>
{
    private readonly IUsuarioRepository _repo;
    public DeleteUsuarioCommandHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteUsuarioCommand request, CancellationToken ct)
    {
        var existe = await _repo.GetById(request.Id, ct);
        if (existe is null) throw new KeyNotFoundException($"Usuario {request.Id} não encontrado.");
        return await _repo.Delete(request.Id, ct);
    }
}
