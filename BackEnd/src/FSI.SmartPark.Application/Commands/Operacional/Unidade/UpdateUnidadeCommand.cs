using FSI.SmartPark.Application.DTOs.Operacional;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Operacional.Unidade;

/// <summary>Command CQRS para atualizar Unidade.</summary>
public sealed record UpdateUnidadeCommand(
    int Id,
    string Nome,
    int NumeroVagas,
    int DiaVencimento
) : IRequest<UnidadeResponseDto>;
