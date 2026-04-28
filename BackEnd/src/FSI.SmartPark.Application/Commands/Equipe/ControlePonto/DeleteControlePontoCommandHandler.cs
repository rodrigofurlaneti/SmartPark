using FSI.SmartPark.Domain.Interfaces.Equipe;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Equipe.ControlePonto;

public sealed class DeleteControlePontoCommandHandler
    : IRequestHandler<DeleteControlePontoCommand, bool>
{
    private readonly IControlePontoRepository _repo;
    public DeleteControlePontoCommandHandler(IControlePontoRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteControlePontoCommand request, CancellationToken ct)
    {
        var existe = await _repo.GetById(request.Id, ct);
        if (existe is null) throw new KeyNotFoundException($"ControlePonto {request.Id} não encontrado.");
        return await _repo.Delete(request.Id, ct);
    }
}
