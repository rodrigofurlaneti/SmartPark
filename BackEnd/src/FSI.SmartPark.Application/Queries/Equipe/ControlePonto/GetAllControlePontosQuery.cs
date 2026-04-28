using FSI.SmartPark.Application.DTOs.Equipe;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Equipe.ControlePonto;

/// <summary>Lista todos os registros ativos de ControlePonto.</summary>
public sealed record GetAllControlePontosQuery() : IRequest<IEnumerable<ControlePontoResponseDto>>;
