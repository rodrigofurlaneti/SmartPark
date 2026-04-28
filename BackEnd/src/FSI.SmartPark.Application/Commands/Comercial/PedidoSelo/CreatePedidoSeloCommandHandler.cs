using FSI.SmartPark.Application.DTOs.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.PedidoSelo;

public sealed class CreatePedidoSeloCommandHandler
    : IRequestHandler<CreatePedidoSeloCommand, PedidoSeloResponseDto>
{
    private readonly IPedidoSeloRepository _repo;
    public CreatePedidoSeloCommandHandler(IPedidoSeloRepository repo) => _repo = repo;

    public async Task<PedidoSeloResponseDto> Handle(CreatePedidoSeloCommand request, CancellationToken ct)
    {
        var entidade = new FSI.SmartPark.Domain.Entities.Comercial.PedidoSelo(request.Quantidade, request.ClienteId, request.UnidadeId,
            request.TipoSeloId, request.FormaPagamento, request.TipoPedido, request.UsuarioId, request.DiasVencimento);
        if (request.ConvenioId.HasValue) entidade.VincularConvenio(request.ConvenioId.Value);
        var id = await _repo.Add(entidade, ct);
        var criado = await _repo.GetById(id, ct) ?? throw new InvalidOperationException("PedidoSelo não encontrado após inserção.");
        return ToDto(criado);
    }

    private static PedidoSeloResponseDto ToDto(FSI.SmartPark.Domain.Entities.Comercial.PedidoSelo e) => new PedidoSeloResponseDto(e.Id, e.Quantidade, e.StatusPedido, e.Cliente_Id, e.Unidade_Id, e.TipoSelo_Id, e.DataVencimento, e.DataInsercao);
}
