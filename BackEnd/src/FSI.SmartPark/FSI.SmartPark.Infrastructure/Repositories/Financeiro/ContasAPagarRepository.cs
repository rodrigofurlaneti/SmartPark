using FSI.SmartPark.Domain.Entities.Financeiro;
using FSI.SmartPark.Domain.Interfaces.Financeiro;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Financeiro;

public class ContasAPagarRepository : RepositoryBase<ContasAPagar>, IContasAPagarRepository
{
    protected override string Tabela => "ContasAPagar";
    public ContasAPagarRepository(SmartParkDbContext ctx) : base(ctx) { }
}
