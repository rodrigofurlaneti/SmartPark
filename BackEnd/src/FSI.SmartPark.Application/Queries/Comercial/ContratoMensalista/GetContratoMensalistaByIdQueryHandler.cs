using FSI.SmartPark.Application.DTOs.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Domain.Entities.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Comercial.ContratoMensalista;

public sealed class GetContratoMensalistaByIdQueryHandler
    : IRequestHandler<GetContratoMensalistaByIdQuery, ContratoMensalistaResponseDto?>
{
    private readonly IContratoMensalistaRepository _repo;
    public GetContratoMensalistaByIdQueryHandler(IContratoMensalistaRepository repo) => _repo = repo;

    public async Task<ContratoMensalistaResponseDto?> Handle(GetContratoMensalistaByIdQuery request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct);
        return entidade is null ? null : ToDto(entidade);
    }

    private static ContratoMensalistaResponseDto ToDto(ContratoMensalista e) => new ContratoMensalistaResponseDto(e.Id, e.NumeroContrato, e.Cliente_Id, e.Unidade_Id, e.Valor, e.Ativo, e.DataVencimento, e.NumeroVagas);