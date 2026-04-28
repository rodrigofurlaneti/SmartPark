using FSI.SmartPark.Domain.Interfaces.Equipe;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Equipe.Funcionario;

public sealed class DeleteFuncionarioCommandHandler
    : IRequestHandler<DeleteFuncionarioCommand, bool>
{
    private readonly IFuncionarioRepository _repo;
    public DeleteFuncionarioCommandHandler(IFuncionarioRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteFuncionarioCommand request, CancellationToken ct)
    {
        var existe = await _repo.GetById(request.Id, ct);
        if (existe is null) throw new KeyNotFoundException($"Funcionario {request.Id} não encontrado.");
        return await _repo.Delete(request.Id, ct);
    }
}
