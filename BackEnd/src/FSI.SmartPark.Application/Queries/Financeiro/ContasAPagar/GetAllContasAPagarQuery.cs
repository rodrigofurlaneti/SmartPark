using FSI.SmartPark.Application.DTOs.Financeiro;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Financeiro.ContasAPagar;

/// <summary>Lista todos os registros ativos de ContasAPagar.</summary>
public sealed record GetAllContasAPagarQuery() : IRequest<IEnumerable<ContasAPagarResponseDto>>;
