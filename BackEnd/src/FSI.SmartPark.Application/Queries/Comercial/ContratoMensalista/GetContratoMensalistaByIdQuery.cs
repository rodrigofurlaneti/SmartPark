using FSI.SmartPark.Application.DTOs.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Comercial.ContratoMensalista;

/// <summary>Busca ContratoMensalista por Id.</summary>
public sealed record GetContratoMensalistaByIdQuery(int Id) : IRequest<ContratoMensalistaResponseDto?>;
