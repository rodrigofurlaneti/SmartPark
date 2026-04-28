using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Application.Interfaces.Operacional;
using FSI.SmartPark.Domain.Entities.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;

namespace FSI.SmartPark.Application.Services.Operacional;

public class MovimentacaoService : IMovimentacaoService
{
    private readonly IMovimentacaoRepository _repo;

    public MovimentacaoService(IMovimentacaoRepository repo) => _repo = repo;

    public async Task<MovimentacaoResponseDto> RegistrarEntrada(EntradaVeiculoRequestDto dto)
    {
        var mov = new Movimentacao(dto.Placa, dto.UnidadeId);

        if (dto.ClienteId.HasValue)
            mov.VincularCliente(dto.ClienteId.Value, dto.NumeroContrato);

        var id = await _repo.Inserir(mov);
        var criado = await _repo.ObterPorId(id);
        return ToDto(criado!);
    }

    public async Task<MovimentacaoResponseDto> RegistrarSaida(SaidaVeiculoRequestDto dto)
    {
        var mov = await _repo.ObterPorId(dto.MovimentacaoId)
            ?? throw new KeyNotFoundException($"Movimentação {dto.MovimentacaoId} não encontrada.");

        mov.RegistrarSaida(dto.ValorCobrado, dto.FormaPagamento);

        if (dto.CpfParaNF is not null)
            mov.InformarCpfParaNF(dto.CpfParaNF);

        await _repo.RegistrarSaida(dto.MovimentacaoId, dto.ValorCobrado);
        return ToDto(mov);
    }

    public async Task<MovimentacaoResponseDto?> ObterPorId(int id)
    {
        var mov = await _repo.ObterPorId(id);
        return mov is null ? null : ToDto(mov);
    }

    public async Task<IEnumerable<MovimentacaoResponseDto>> ListarAbertas(int unidadeId)
    {
        var lista = await _repo.ListarTodos();
        return lista
            .Where(m => m.Unidade_Id == unidadeId && !m.DataSaida.HasValue)
            .Select(ToDto);
    }

    public async Task<IEnumerable<MovimentacaoResponseDto>> ListarPorPeriodo(int unidadeId, DateTime inicio, DateTime fim)
    {
        var lista = await _repo.ObterPorPeriodo(unidadeId, inicio, fim);
        return lista.Select(ToDto);
    }

    private static MovimentacaoResponseDto ToDto(Movimentacao m) => new(
        m.Id, m.Ticket, m.Placa, m.DataEntrada, m.DataSaida,
        m.ValorCobrado, m.FormaPagamento, m.TipoCliente,
        m.Unidade_Id, !m.DataSaida.HasValue);
}
