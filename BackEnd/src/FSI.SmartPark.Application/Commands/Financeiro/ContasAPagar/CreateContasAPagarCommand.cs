using FSI.SmartPark.Application.DTOs.Financeiro;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Financeiro.ContasAPagar;

/// <summary>Command CQRS para criar ContasAPagar.</summary>
public sealed record CreateContasAPagarCommand(
    string NumeroDocumento,
    DateTime DataVencimento,
    decimal ValorTotal,
    int EmpresaId,
    int? FornecedorId,
    int? UnidadeId
) : IRequest<ContasAPagarResponseDto>;
