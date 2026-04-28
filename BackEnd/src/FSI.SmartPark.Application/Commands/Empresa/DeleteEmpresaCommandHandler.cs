using FSI.SmartPark.Domain.Interfaces.Identidade;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Empresa;

/// <summary>
/// Handler do DeleteEmpresaCommand.
/// Executa Soft Delete — IsDeleted = true, DeletedAt = UTC_NOW.
/// Nunca remove o registro fisicamente do banco.
/// </summary>
public sealed class DeleteEmpresaCommandHandler
    : IRequestHandler<DeleteEmpresaCommand, bool>
{
    private readonly IEmpresaRepository _repo;

    public DeleteEmpresaCommandHandler(IEmpresaRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteEmpresaCommand request, CancellationToken ct)
    {
        // Verifica existência antes de tentar excluir
        var existe = await _repo.GetById(request.Id, ct);
        if (existe is null)
            throw new KeyNotFoundException($"Empresa {request.Id} não encontrada.");

        // Soft Delete via RepositoryBase (IsDeleted=1, DeletedAt=UTC_NOW)
        return await _repo.Delete(request.Id, ct);
    }
}
