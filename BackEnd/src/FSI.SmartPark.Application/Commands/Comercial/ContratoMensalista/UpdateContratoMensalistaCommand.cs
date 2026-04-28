using FSI.SmartPark.Application.DTOs.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.ContratoMensalista;

/// <summary>Command CQRS para atualizar ContratoMensalista.</summary>
public sealed record UpdateContratoMensalistaCommand(
    int Id,
    decimal Valor
) : IRequest<ContratoMensalistaResponseDto>;
