using FSI.SmartPark.Application.DTOs.Suporte;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Suporte.Estoque;

/// <summary>Command CQRS para atualizar Estoque.</summary>
public sealed record UpdateEstoqueCommand(
    int Id,
    string Nome,
    bool EstoquePrincipal
) : IRequest<EstoqueResponseDto>;
