using FSI.SmartPark.Application.DTOs.Equipe;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Equipe.Funcionario;

/// <summary>Command CQRS para atualizar Funcionario.</summary>
public sealed record UpdateFuncionarioCommand(
    int Id,
    decimal Salario
) : IRequest<FuncionarioResponseDto>;
