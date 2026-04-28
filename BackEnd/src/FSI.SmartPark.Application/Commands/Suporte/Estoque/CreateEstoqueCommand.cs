using FSI.SmartPark.Application.DTOs.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Suporte.Estoque;

/// <summary>Command CQRS para criar Estoque.</summary>
public sealed record CreateEstoqueCommand(
    string Nome,
    int EmpresaId,
    int? UnidadeId,
    bool EstoquePrincipal
) : IRequest<EstoqueResponseDto>;
