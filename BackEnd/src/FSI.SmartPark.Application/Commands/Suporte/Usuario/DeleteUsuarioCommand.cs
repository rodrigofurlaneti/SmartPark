using MediatR;

namespace FSI.SmartPark.Application.Commands.Suporte.Usuario;

/// <summary>Soft Delete de Usuario (IsDeleted = true).</summary>
public sealed record DeleteUsuarioCommand(int Id) : IRequest<bool>;
