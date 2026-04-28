using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Application.Interfaces.Operacional;
using FSI.SmartPark.Domain.Entities.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;

namespace FSI.SmartPark.Application.Services.Operacional;

public class FaturamentoService : IFaturamentoService
{
    private readonly IFaturamentoRepository _repo;

    public FaturamentoService(IFaturamentoRepository repo) => _repo = repo;

    public async Task<FaturamentoResponseDto> AbrirTurno(AbrirTurnoRequestDto dto)
    {
        var fat = new Faturamento(dto.NumFechamento, dto.NumTerminal, dto.UnidadeId, dto.UsuarioId);
        fat.DefinirSaldoInicial(dto.SaldoInicial);
        var id = await _repo.Inserir(fat);
        var criado = await _repo.ObterPorId(id);
        return ToDto(criado!);
    }

    public async Task<FaturamentoResponseDto> FecharTurno(FecharTurnoRequestDto dto)
    {
        var fat = await _repo.ObterPorId(dto.FaturamentoId)
            ?? throw new KeyNotFoundException($"Faturamento {dto.FaturamentoId} não encontrado.");

        fat.FecharTurno(dto.ValorTotal, dto.ValorDinheiro, dto.ValorCartaoDebito,
            dto.ValorCartaoCredito, dto.ValorRotativo, dto.ValorMensalidade,
            dto.ValorSemParar, dto.ValorSeloDesconto, dto.TicketFinal);

        await _repo.Atualizar(fat);
        return ToDto(fat);
    }

    public async Task<FaturamentoResponseDto?> ObterPorId(int id)
    {
        var fat = await _repo.ObterPorId(id);
        return fat is null ? null : ToDto(fat);
    }

    public async Task<IEnumerable<FaturamentoResponseDto>> ListarPorPeriodo(int unidadeId, DateTime inicio, DateTime fim)
    {
        var lista = await _repo.ObterPorPeriodo(unidadeId, inicio, fim);
        return lista.Select(ToDto);
    }

    public async Task RegistrarSangria(int faturamentoId, decimal valor)
    {
        var fat = await _repo.ObterPorId(faturamentoId)
            ?? throw new KeyNotFoundException($"Faturamento {faturamentoId} não encontrado.");
        fat.RegistrarSangria(valor);
        await _repo.Atualizar(fat);
    }

    private static FaturamentoResponseDto ToDto(Faturamento f) => new(
        f.Id, f.NumFechamento, f.NumTerminal, f.Unidade_Id,
        f.DataAbertura, f.DataFechamento, f.ValorTotal,
        f.ValorDinheiro, f.ValorCartaoDebito, f.ValorCartaoCredito,
        f.ValorRotativo, f.ValorSemParar, f.SaldoInicial, f.ValorSangria, f.Usuario_Id);
}
