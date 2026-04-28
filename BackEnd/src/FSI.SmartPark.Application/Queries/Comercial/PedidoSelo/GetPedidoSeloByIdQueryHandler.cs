using FSI.SmartPark.Application.DTOs.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Queries.Comercial.PedidoSelo;

public sealed class GetPedidoSeloByIdQueryHandler
    : IRequestHandler<GetPedidoSeloByIdQuery, PedidoSeloResponseDto?>
{
    private readonly IPedidoSeloRepository _repo;
    public GetPedidoSeloByIdQueryHandler(IPedidoSeloRepository repo) => _repo = repo;

    public async Task<PedidoSeloResponseDto?> Handle(GetPedidoSeloByIdQuery request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct);
        return entidade is null ? null : ToDto(entidade);
    }

    private static PedidoSeloResponseDto ToDto(FSI.SmartPark.Domain.Entities.Comercial.PedidoSelo e) => new PedidoSeloResponseDto(e.Id, e.Quantidade, e.StatusPedido, e.Cliente_Id, e.Unidade_Id, e.TipoSelo_Id, e.DataVencimento, e.DataInsercao);
}
