using FSI.SmartPark.Application.DTOs.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Movimentacao;

/// <summary>Command CQRS para criar Movimentacao.</summary>
public sealed record CreateMovimentacaoCommand(
    string Placa,
    int UnidadeId,
    int EmpresaId,
    int? ClienteId,
    string? NumeroContrato
) : IRequest<MovimentacaoResponseDto>;
