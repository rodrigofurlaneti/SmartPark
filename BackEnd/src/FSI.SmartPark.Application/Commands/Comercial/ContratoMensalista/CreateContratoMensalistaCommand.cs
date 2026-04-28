using FSI.SmartPark.Application.DTOs.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.ContratoMensalista;

/// <summary>Command CQRS para criar ContratoMensalista.</summary>
public sealed record CreateContratoMensalistaCommand(
    int ClienteId,
    int UnidadeId,
    decimal Valor,
    int EmpresaId
) : IRequest<ContratoMensalistaResponseDto>;
