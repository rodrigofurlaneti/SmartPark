using FSI.SmartPark.Application.DTOs.Equipe;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Equipe.Funcionario;

/// <summary>Lista todos os registros ativos de Funcionario.</summary>
public sealed record GetAllFuncionariosQuery() : IRequest<IEnumerable<FuncionarioResponseDto>>;
