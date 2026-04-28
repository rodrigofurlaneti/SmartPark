using FSI.SmartPark.Domain.Entities.Operacional;

namespace FSI.SmartPark.Domain.Interfaces.Operacional
{
    public interface IMovimentacaoRepository : IRepositoryBase<Movimentacao>
    {
        Task<Movimentacao> ObterAtivoPorPlaca(string placa, int unidadeId);
        Task<bool> RegistrarSaida(int id, decimal valorCobrado);
        Task<IEnumerable<Movimentacao>> ObterPorPeriodo(int unidadeId, DateTime inicio, DateTime fim);
    }
}
