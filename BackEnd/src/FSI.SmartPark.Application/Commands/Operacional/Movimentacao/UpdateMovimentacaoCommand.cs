using FSI.SmartPark.Application.DTOs.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Movimentacao;

/// <summary>Command CQRS para atualizar Movimentacao.</summary>
public sealed record UpdateMovimentacaoCommand(
    int Id,
    decimal ValorCobrado,
    string? FormaPagamento,
    string? CpfParaNF
) : IRequest<MovimentacaoResponseDto>;
