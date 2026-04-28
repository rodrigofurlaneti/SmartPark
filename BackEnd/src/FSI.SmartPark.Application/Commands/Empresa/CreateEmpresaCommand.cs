using FSI.SmartPark.Application.DTOs.Identidade;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Empresa;

/// <summary>
/// Command CQRS para criar uma nova Empresa (tenant) no sistema.
/// Implementa IRequest{EmpresaResponseDto} — o Handler retorna o DTO criado.
/// </summary>
public sealed record CreateEmpresaCommand(
    string NomeFantasia,
    string Cnpj,
    string? RazaoSocial = null,
    string? Email       = null,
    string? Telefone    = null
) : IRequest<EmpresaResponseDto>;
