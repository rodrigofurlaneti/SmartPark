using FSI.SmartPark.Domain.Interfaces.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.ContratoMensalista;

public sealed class DeleteContratoMensalistaCommandHandler
    : IRequestHandler<DeleteContratoMensalistaCommand, bool>
{
    private readonly IContratoMensalistaRepository _repo;
    public DeleteContratoMensalistaCommandHandler(IContratoMensalistaRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteContratoMensalistaCommand request, CancellationToken ct)
    {
        var existe = await _repo.GetById(request.Id, ct);
        if (existe is null) throw new KeyNotFoundException($"ContratoMensalista {request.Id} não encontrado.");
        return await _repo.Delete(request.Id, ct);
    }
}
