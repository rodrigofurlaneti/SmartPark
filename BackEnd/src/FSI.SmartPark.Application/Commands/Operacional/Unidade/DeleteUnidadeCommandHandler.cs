using FSI.SmartPark.Domain.Interfaces.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Unidade;

public sealed class DeleteUnidadeCommandHandler
    : IRequestHandler<DeleteUnidadeCommand, bool>
{
    private readonly IUnidadeRepository _repo;
    public DeleteUnidadeCommandHandler(IUnidadeRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteUnidadeCommand request, CancellationToken ct)
    {
        var existe = await _repo.GetById(request.Id, ct);
        if (existe is null) throw new KeyNotFoundException($"Unidade {request.Id} não encontrado.");
        return await _repo.Delete(request.Id, ct);
    }
}
