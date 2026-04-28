using FSI.SmartPark.Application.DTOs.Identidade;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Empresa;

/// <summary>
/// Query CQRS para buscar uma Empresa pelo seu Id.
/// Retorna null se não existir ou estiver com IsDeleted = true.
/// </summary>
public sealed record GetEmpresaByIdQuery(int Id) : IRequest<EmpresaResponseDto?>;
