using FSI.SmartPark.Application.DTOs.Financeiro;
namespace FSI.SmartPark.Application.Interfaces.Financeiro;

public interface ILancamentoCobrancaService
{
    Task<LancamentoCobrancaResponseDto?> ObterPorId(int id);
    Task<IEnumerable<LancamentoCobrancaResponseDto>> ListarPorCliente(int clienteId);
    Task<IEnumerable<LancamentoCobrancaResponseDto>> ListarVencidos();
    Task AplicarTaxas(int id, decimal multa, decimal juros);
}
