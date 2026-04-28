using FSI.SmartPark.Domain.Entities.Financeiro;
using FSI.SmartPark.Domain.Interfaces.Financeiro;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Financeiro;

public class ContaCorrenteClienteRepository : RepositoryBase<ContaCorrenteCliente>, IContaCorrenteClienteRepository
{
    protected override string Tabela => "ContaCorrenteCliente";
    public ContaCorrenteClienteRepository(SmartParkDbContext ctx) : base(ctx) { }
}
