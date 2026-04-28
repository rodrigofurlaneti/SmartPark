using MediatR;

namespace FSI.SmartPark.Application.Commands.Empresa;

/// <summary>
/// Command CQRS para excluir logicamente uma Empresa (Soft Delete).
/// O registro permanece no banco com IsDeleted = true.
/// </summary>
public sealed record DeleteEmpresaCommand(int Id) : IRequest<bool>;
