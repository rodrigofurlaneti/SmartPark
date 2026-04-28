using FSI.SmartPark.Domain.Interfaces.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.PedidoSelo;

public sealed class DeletePedidoSeloCommandHandler
    : IRequestHandler<DeletePedidoSeloCommand, bool>
{
    private readonly IPedidoSeloRepository _repo;
    public DeletePedidoSeloCommandHandler(IPedidoSeloRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeletePedidoSeloCommand request, CancellationToken ct)
    {
        var existe = await _repo.GetById(request.Id, ct);
        if (existe is null) throw new KeyNotFoundException($"PedidoSelo {request.Id} não encontrado.");
        return await _repo.Delete(request.Id, ct);
    }
}
