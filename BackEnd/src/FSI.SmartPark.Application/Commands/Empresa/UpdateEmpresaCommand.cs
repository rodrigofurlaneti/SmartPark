using FSI.SmartPark.Application.DTOs.Identidade;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Empresa;

/// <summary>
/// Command CQRS para atualizar os dados cadastrais de uma Empresa existente.
/// </summary>
public sealed record UpdateEmpresaCommand(
    int     Id,
    string  NomeFantasia,
    string? RazaoSocial = null,
    string? Email       = null,
    string? Telefone    = null
) : IRequest<EmpresaResponseDto>;
