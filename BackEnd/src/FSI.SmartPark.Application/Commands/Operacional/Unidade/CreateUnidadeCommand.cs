using FSI.SmartPark.Application.DTOs.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Unidade;

/// <summary>Command CQRS para criar Unidade.</summary>
public sealed record CreateUnidadeCommand(
    string Nome,
    int NumeroVagas,
    int DiaVencimento,
    int EmpresaId,
    string? CNPJ,
    string? CCM
) : IRequest<UnidadeResponseDto>;
