using FSI.SmartPark.Application.DTOs.Comercial;
using FSI.SmartPark.Application.Interfaces.Comercial;
using FSI.SmartPark.Domain.Entities.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;

namespace FSI.SmartPark.Application.Services.Comercial;

public class PedidoSeloService : IPedidoSeloService
{
    private readonly IPedidoSeloRepository _repo;

    public PedidoSeloService(IPedidoSeloRepository repo) => _repo = repo;

    public async Task<PedidoSeloResponseDto> Criar(PedidoSeloRequestDto dto)
    {
        var pedido = new PedidoSelo(
            dto.Quantidade, dto.ClienteId, dto.UnidadeId,
            dto.TipoSeloId, dto.FormaPagamento, dto.TipoPedido,
            dto.UsuarioId, dto.DiasVencimento);

        if (dto.ConvenioId.HasValue)
            pedido.VincularConvenio(dto.ConvenioId.Value);

        var id = await _repo.Inserir(pedido);
        var criado = await _repo.ObterPorId(id);
        return ToDto(criado!);
    }

    public async Task<PedidoSeloResponseDto?> ObterPorId(int id)
    {
        var p = await _repo.ObterPorId(id);
        return p is null ? null : ToDto(p);
    }

    public async Task<IEnumerable<PedidoSeloResponseDto>> ListarPorCliente(int clienteId)
    {
        var lista = await _repo.ListarTodos();
        return lista.Where(p => p.Cliente_Id == clienteId).Select(ToDto);
    }

    public async Task<IEnumerable<PedidoSeloResponseDto>> ListarPendentes(int unidadeId)
    {
        var lista = await _repo.ListarTodos();
        return lista
            .Where(p => p.Unidade_Id == unidadeId && p.StatusPedido == Domain.Enums.StatusPedidoSelo.Pendente)
            .Select(ToDto);
    }

    public async Task Cancelar(int id)
    {
        var p = await _repo.ObterPorId(id)
            ?? throw new KeyNotFoundException($"Pedido de selo {id} não encontrado.");
        p.Cancelar();
        await _repo.Atualizar(p);
    }

    private static PedidoSeloResponseDto ToDto(PedidoSelo p) => new(
        p.Id, p.Quantidade, p.StatusPedido, p.Cliente_Id,
        p.Unidade_Id, p.TipoSelo_Id, p.DataVencimento, p.DataInsercao);
}
