using FSI.SmartPark.Application.DTOs.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Faturamento;

/// <summary>Command CQRS para criar Faturamento.</summary>
public sealed record CreateFaturamentoCommand(
    int NumFechamento,
    int NumTerminal,
    int UnidadeId,
    int UsuarioId,
    int EmpresaId,
    decimal SaldoInicial
) : IRequest<FaturamentoResponseDto>;
