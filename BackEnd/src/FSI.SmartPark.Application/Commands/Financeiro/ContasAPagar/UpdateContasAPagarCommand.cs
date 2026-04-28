using FSI.SmartPark.Application.DTOs.Financeiro;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Financeiro.ContasAPagar;

/// <summary>Command CQRS para atualizar ContasAPagar.</summary>
public sealed record UpdateContasAPagarCommand(
    int Id,
    DateTime DataVencimento,
    decimal ValorTotal
) : IRequest<ContasAPagarResponseDto>;
