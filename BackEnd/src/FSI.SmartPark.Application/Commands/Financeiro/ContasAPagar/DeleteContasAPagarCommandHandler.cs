using FSI.SmartPark.Domain.Interfaces.Financeiro;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Financeiro.ContasAPagar;

public sealed class DeleteContasAPagarCommandHandler
    : IRequestHandler<DeleteContasAPagarCommand, bool>
{
    private readonly IContasAPagarRepository _repo;
    public DeleteContasAPagarCommandHandler(IContasAPagarRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteContasAPagarCommand request, CancellationToken ct)
    {
        var existe = await _repo.GetById(request.Id, ct);
        if (existe is null) throw new KeyNotFoundException($"ContasAPagar {request.Id} não encontrado.");
        return await _repo.Delete(request.Id, ct);
    }
}
