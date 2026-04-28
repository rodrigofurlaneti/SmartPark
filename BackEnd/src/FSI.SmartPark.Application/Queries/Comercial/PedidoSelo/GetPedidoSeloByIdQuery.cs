using FSI.SmartPark.Application.DTOs.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Comercial.PedidoSelo;

/// <summary>Busca PedidoSelo por Id.</summary>
public sealed record GetPedidoSeloByIdQuery(int Id) : IRequest<PedidoSeloResponseDto?>;
