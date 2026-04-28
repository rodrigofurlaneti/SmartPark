using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.ContratoMensalista;

/// <summary>Soft Delete de ContratoMensalista (IsDeleted = true).</summary>
public sealed record DeleteContratoMensalistaCommand(int Id) : IRequest<bool>;
