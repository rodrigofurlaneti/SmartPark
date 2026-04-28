using FSI.SmartPark.Domain.Entities.Operacional;
namespace FSI.SmartPark.Domain.Interfaces.Operacional
{
    public interface IFaturamentoRepository : IRepositoryBase<Faturamento>
    {
        Task<IEnumerable<Faturamento>> ObterPorPeriodo(int unidadeId, DateTime inicio, DateTime fim);
    }
}
