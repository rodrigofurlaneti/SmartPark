using FSI.SmartPark.Application.DTOs.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Domain.Entities.Comercial;
using MediatR;

namespace FSI.SmartPark.Application.Commands.Comercial.PedidoSelo;

public sealed class UpdatePedidoSeloCommandHandler
    : IRequestHandler<UpdatePedidoSeloCommand, PedidoSeloResponseDto>
{
    private readonly IPedidoSeloRepository _repo;
    public UpdatePedidoSeloCommandHandler(IPedidoSeloRepository repo) => _repo = repo;

    public async Task<PedidoSeloResponseDto> Handle(UpdatePedidoSeloCommand request, CancellationToken ct)
    {
        var entidade = await _repo.GetById(request.Id, ct)
            ?? throw new KeyNotFoundException($"PedidoSelo {request.Id} não encontrado.");
        entidade.Cancelar();
        await _repo.Update(entidade, ct);
        return ToDto(entidade);
    }

    private static PedidoSeloResponseDto ToDto(PedidoSelo e) => new PedidoSeloResponseDto(e.Id, e.Quantidade, e.StatusPedido, e.Cliente_Id, e.Unidade_Id, e.TipoSelo_Id, e.DataVencimento, e.DataInsercao);