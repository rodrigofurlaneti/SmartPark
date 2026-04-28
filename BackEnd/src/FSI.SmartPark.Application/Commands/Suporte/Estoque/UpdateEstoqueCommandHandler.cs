using FSI.SmartPark.Application.DTOs.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Suporte.Estoque;

public sealed class UpdateEstoqueCommandHandler
    : IRequestHandler<UpdateEstoqueCommand, EstoqueResponseDto>
{
    private readonly IEstoqueRepository _repo;
    public UpdateEstoqueCommandHandler(IEstoqueRepository repo) => _repo = repo;

    public async Task<EstoqueResponseDto> Handle(UpdateEstoqueCommand request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct)
            ?? throw new KeyNotFoundException($"Estoque {request.Id} não encontrado.");
        await _repo.Update(entidade, ct);
        return ToDto(entidade);
    }

    private static EstoqueResponseDto ToDto(FSI.SmartPark.Domain.Entities.Suporte.Estoque e) => new EstoqueResponseDto(e.Id, e.Nome, e.EstoquePrincipal, e.Unidade_Id);
}
