using FSI.SmartPark.Domain.Interfaces.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Suporte.Estoque;

public sealed class DeleteEstoqueCommandHandler
    : IRequestHandler<DeleteEstoqueCommand, bool>
{
    private readonly IEstoqueRepository _repo;
    public DeleteEstoqueCommandHandler(IEstoqueRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteEstoqueCommand request, CancellationToken ct)
    {
        var existe = await _repo.GetById(request.Id, ct);
        if (existe is null) throw new KeyNotFoundException($"Estoque {request.Id} não encontrado.");
        return await _repo.Delete(request.Id, ct);
    }
}
