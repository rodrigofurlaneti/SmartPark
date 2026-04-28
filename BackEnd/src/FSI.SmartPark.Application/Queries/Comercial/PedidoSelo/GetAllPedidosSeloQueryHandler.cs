using FSI.SmartPark.Application.DTOs.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Domain.Entities.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Comercial.PedidoSelo;

public sealed class GetAllPedidosSeloQueryHandler
    : IRequestHandler<GetAllPedidosSeloQuery, IEnumerable<PedidoSeloResponseDto>>
{
    private readonly IPedidoSeloRepository _repo;
    public GetAllPedidosSeloQueryHandler(IPedidoSeloRepository repo) => _repo = repo;

    public async Task<IEnumerable<PedidoSeloResponseDto>> Handle(GetAllPedidosSeloQuery request, CancellationToken ct)
    {
        var lista = await _repo.GetAll(ct);
        return lista.Select(ToDto);
    }

    private static PedidoSeloResponseDto ToDto(PedidoSelo e) => new PedidoSeloResponseDto(e.Id, e.Quantidade, e.StatusPedido, e.Cliente_Id, e.Unidade_Id, e.TipoSelo_Id, e.DataVencimento, e.DataInsercao);