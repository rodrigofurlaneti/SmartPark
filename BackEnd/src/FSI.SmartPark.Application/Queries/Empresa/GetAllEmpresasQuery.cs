using FSI.SmartPark.Application.DTOs.Identidade;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Empresa;

/// <summary>
/// Query CQRS para listar todas as Empresas ativas (IsDeleted = false).
/// </summary>
public sealed record GetAllEmpresasQuery() : IRequest<IEnumerable<EmpresaResponseDto>>;
