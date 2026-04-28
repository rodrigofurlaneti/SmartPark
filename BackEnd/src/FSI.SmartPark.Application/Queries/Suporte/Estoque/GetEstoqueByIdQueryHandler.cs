using FSI.SmartPark.Application.DTOs.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using FSI.SmartPark.Domain.Entities.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Suporte.Estoque;

public sealed class GetEstoqueByIdQueryHandler
    : IRequestHandler<GetEstoqueByIdQuery, EstoqueResponseDto?>
{
    private readonly IEstoqueRepository _repo;
    public GetEstoqueByIdQueryHandler(IEstoqueRepository repo) => _repo = repo;

    public async Task<EstoqueResponseDto?> Handle(GetEstoqueByIdQuery request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct);
        return entidade is null ? null : ToDto(entidade);
    }

    private static EstoqueResponseDto ToDto(Estoque e) => new EstoqueResponseDto(e.Id, e.Nome, e.EstoquePrincipal, e.Unidade_Id);
}
