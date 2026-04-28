using FSI.SmartPark.Application.DTOs.Equipe;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Equipe.ControlePonto;

/// <summary>Command CQRS para criar ControlePonto.</summary>
public sealed record CreateControlePontoCommand(
    int FuncionarioId,
    FSI.SmartPark.Domain.Enums.TipoRegistroPonto TipoRegistro,
    int? UnidadeId
) : IRequest<ControlePontoResponseDto>;
