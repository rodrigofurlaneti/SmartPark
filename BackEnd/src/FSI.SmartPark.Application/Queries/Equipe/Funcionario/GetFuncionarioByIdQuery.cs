using FSI.SmartPark.Application.DTOs.Equipe;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Equipe.Funcionario;

/// <summary>Busca Funcionario por Id.</summary>
public sealed record GetFuncionarioByIdQuery(int Id) : IRequest<FuncionarioResponseDto?>;
