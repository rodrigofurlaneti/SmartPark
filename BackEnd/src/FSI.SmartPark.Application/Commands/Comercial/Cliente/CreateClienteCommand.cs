using FSI.SmartPark.Application.DTOs.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.Cliente;

/// <summary>Command CQRS para criar Cliente.</summary>
public sealed record CreateClienteCommand(
    string Nome,
    string Documento,
    bool IsMensalista,
    int EmpresaId
) : IRequest<ClienteResponseDto>;
