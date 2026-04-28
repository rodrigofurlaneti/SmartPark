using FSI.SmartPark.Domain.Interfaces.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.Cliente;

public sealed class DeleteClienteCommandHandler
    : IRequestHandler<DeleteClienteCommand, bool>
{
    private readonly IClienteRepository _repo;
    public DeleteClienteCommandHandler(IClienteRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteClienteCommand request, CancellationToken ct)
    {
        var existe = await _repo.GetById(request.Id, ct);
        if (existe is null) throw new KeyNotFoundException($"Cliente {request.Id} não encontrado.");
        return await _repo.Delete(request.Id, ct);
    }
}
