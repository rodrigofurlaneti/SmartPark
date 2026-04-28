using FSI.SmartPark.Domain.Entities.Financeiro;
using FSI.SmartPark.Domain.Interfaces.Financeiro;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Financeiro;

public class ContaFinanceiraRepository : RepositoryBase<ContaFinanceira>, IContaFinanceiraRepository
{
    protected override string Tabela => "ContaFinanceira";
    public ContaFinanceiraRepository(SmartParkDbContext ctx) : base(ctx) { }
}
