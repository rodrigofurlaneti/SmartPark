using FSI.SmartPark.Domain.Interfaces.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Faturamento;

public sealed class DeleteFaturamentoCommandHandler
    : IRequestHandler<DeleteFaturamentoCommand, bool>
{
    private readonly IFaturamentoRepository _repo;
    public DeleteFaturamentoCommandHandler(IFaturamentoRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteFaturamentoCommand request, CancellationToken ct)
    {
        var existe = await _repo.GetById(request.Id, ct);
        if (existe is null) throw new KeyNotFoundException($"Faturamento {request.Id} não encontrado.");
        return await _repo.Delete(request.Id, ct);
    }
}
