using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Operacional.Unidade;

public sealed class GetAllUnidadesQueryHandler
    : IRequestHandler<GetAllUnidadesQuery, IEnumerable<UnidadeResponseDto>>
{
    private readonly IUnidadeRepository _repo;
    public GetAllUnidadesQueryHandler(IUnidadeRepository repo) => _repo = repo;

    public async Task<IEnumerable<UnidadeResponseDto>> Handle(GetAllUnidadesQuery request, CancellationToken ct)
    {
        var lista = await _repo.GetAll(ct);
        return lista.Select(ToDto);
    }

    private static UnidadeResponseDto ToDto(FSI.SmartPark.Domain.Entities.Operacional.Unidade e) => new UnidadeResponseDto(e.Id, e.Codigo, e.Nome, e.NumeroVaga, e.Ativa, e.DiaVencimento, e.CNPJ, e.Empresa_Id, e.Funcionario_Id, e.Endereco_Id);
}
