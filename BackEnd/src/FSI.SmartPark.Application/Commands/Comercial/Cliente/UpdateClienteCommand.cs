using FSI.SmartPark.Application.DTOs.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.Cliente;

/// <summary>Command CQRS para atualizar Cliente.</summary>
public sealed record UpdateClienteCommand(
    int Id,
    string Nome,
    bool IsMensalista
) : IRequest<ClienteResponseDto>;
