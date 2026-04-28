using FSI.SmartPark.Application.DTOs.Identidade;
using FSI.SmartPark.Domain.Interfaces.Identidade;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Empresa;

/// <summary>
/// Handler da GetAllEmpresasQuery.
/// Retorna apenas empresas com IsDeleted = false.
/// </summary>
public sealed class GetAllEmpresasQueryHandler
    : IRequestHandler<GetAllEmpresasQuery, IEnumerable<EmpresaResponseDto>>
{
    private readonly IEmpresaRepository _repo;

    public GetAllEmpresasQueryHandler(IEmpresaRepository repo) => _repo = repo;

    public async Task<IEnumerable<EmpresaResponseDto>> Handle(
        GetAllEmpresasQuery request,
        CancellationToken ct)
    {
        var empresas = await _repo.GetAll(ct);
        return empresas.Select(EmpresaResponseDto.FromEntity);
    }
}
