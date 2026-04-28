using MediatR;

namespace FSI.SmartPark.Application.Commands.Equipe.Funcionario;

/// <summary>Soft Delete de Funcionario (IsDeleted = true).</summary>
public sealed record DeleteFuncionarioCommand(int Id) : IRequest<bool>;
