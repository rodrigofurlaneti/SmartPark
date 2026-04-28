using FSI.SmartPark.Application.DTOs.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Comercial.ContratoMensalista;

/// <summary>Lista todos os registros ativos de ContratoMensalista.</summary>
public sealed record GetAllContratosMensalistasQuery() : IRequest<IEnumerable<ContratoMensalistaResponseDto>>;
