using FSI.SmartPark.Application.DTOs.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Suporte.Estoque;

public sealed class GetAllEstoquesQueryHandler
    : IRequestHandler<GetAllEstoquesQuery, IEnumerable<EstoqueResponseDto>>
{
    private readonly IEstoqueRepository _repo;
    public GetAllEstoquesQueryHandler(IEstoqueRepository repo) => _repo = repo;

    public async Task<IEnumerable<EstoqueResponseDto>> Handle(GetAllEstoquesQuery request, CancellationToken ct)
    {
        var lista = await _repo.GetAll(ct);
        return lista.Select(ToDto);
    }

    private static EstoqueResponseDto ToDto(FSI.SmartPark.Domain.Entities.Suporte.Estoque e) => new EstoqueResponseDto(e.Id, e.Nome, e.EstoquePrincipal, e.Unidade_Id);
}
