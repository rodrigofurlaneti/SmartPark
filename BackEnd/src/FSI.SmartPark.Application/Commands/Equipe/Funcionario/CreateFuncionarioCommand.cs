using FSI.SmartPark.Application.DTOs.Equipe;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Equipe.Funcionario;

/// <summary>Command CQRS para criar Funcionario.</summary>
public sealed record CreateFuncionarioCommand(
    int PessoaId,
    decimal Salario,
    FSI.SmartPark.Domain.Enums.TipoEscalaFuncionario Escala,
    int EmpresaId
) : IRequest<FuncionarioResponseDto>;
