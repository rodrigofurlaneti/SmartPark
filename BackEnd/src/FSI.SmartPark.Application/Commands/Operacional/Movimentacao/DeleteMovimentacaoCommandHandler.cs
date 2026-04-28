using FSI.SmartPark.Domain.Interfaces.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Movimentacao;

public sealed class DeleteMovimentacaoCommandHandler
    : IRequestHandler<DeleteMovimentacaoCommand, bool>
{
    private readonly IMovimentacaoRepository _repo;
    public DeleteMovimentacaoCommandHandler(IMovimentacaoRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteMovimentacaoCommand request, CancellationToken ct)
    {
        var existe = await _repo.GetById(request.Id, ct);
        if (existe is null) throw new KeyNotFoundException($"Movimentacao {request.Id} não encontrado.");
        return await _repo.Delete(request.Id, ct);
    }
}
