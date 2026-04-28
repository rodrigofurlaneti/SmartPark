using FSI.SmartPark.Application.DTOs.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Comercial.PedidoSelo;

/// <summary>Lista todos os registros ativos de PedidoSelo.</summary>
public sealed record GetAllPedidosSeloQuery() : IRequest<IEnumerable<PedidoSeloResponseDto>>;
