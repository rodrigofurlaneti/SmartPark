using FSI.SmartPark.Application.DTOs.Identidade;
using FSI.SmartPark.Domain.Interfaces.Identidade;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Empresa;

/// <summary>
/// Handler da GetEmpresaByIdQuery.
/// Usa AsNoTracking implicitamente (Dapper é sempre read-only por natureza).
/// </summary>
public sealed class GetEmpresaByIdQueryHandler
    : IRequestHandler<GetEmpresaByIdQuery, EmpresaResponseDto?>
{
    private readonly IEmpresaRepository _repo;

    public GetEmpresaByIdQueryHandler(IEmpresaRepository repo) => _repo = repo;

    public async Task<EmpresaResponseDto?> Handle(
        GetEmpresaByIdQuery request,
        CancellationToken ct)
    {
        var empresa = await _repo.GetById(request.Id, ct);
        return empresa is null ? null : EmpresaResponseDto.FromEntity(empresa);
    }
}
