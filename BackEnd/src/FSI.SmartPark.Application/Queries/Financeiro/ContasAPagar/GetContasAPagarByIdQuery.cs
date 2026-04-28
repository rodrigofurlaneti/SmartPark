using FSI.SmartPark.Application.DTOs.Financeiro;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Financeiro.ContasAPagar;

/// <summary>Busca ContasAPagar por Id.</summary>
public sealed record GetContasAPagarByIdQuery(int Id) : IRequest<ContasAPagarResponseDto?>;
