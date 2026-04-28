using FSI.SmartPark.Application.DTOs.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Suporte.Estoque;

/// <summary>Busca Estoque por Id.</summary>
public sealed record GetEstoqueByIdQuery(int Id) : IRequest<EstoqueResponseDto?>;
