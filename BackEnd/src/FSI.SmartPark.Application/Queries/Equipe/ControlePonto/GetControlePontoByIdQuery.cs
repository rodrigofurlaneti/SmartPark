using FSI.SmartPark.Application.DTOs.Equipe;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Equipe.ControlePonto;

/// <summary>Busca ControlePonto por Id.</summary>
public sealed record GetControlePontoByIdQuery(int Id) : IRequest<ControlePontoResponseDto?>;
