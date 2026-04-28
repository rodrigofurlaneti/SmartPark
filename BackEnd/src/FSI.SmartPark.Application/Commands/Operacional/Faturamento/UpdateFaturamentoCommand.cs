using FSI.SmartPark.Application.DTOs.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Faturamento;

/// <summary>Command CQRS para atualizar Faturamento.</summary>
public sealed record UpdateFaturamentoCommand(
    int Id,
    decimal ValorTotal,
    decimal ValorDinheiro,
    decimal ValorCartaoDebito,
    decimal ValorCartaoCredito,
    decimal? ValorRotativo,
    decimal? ValorMensalidade,
    decimal? ValorSemParar,
    decimal? ValorSeloDesconto,
    string? TicketFinal
) : IRequest<FaturamentoResponseDto>;
