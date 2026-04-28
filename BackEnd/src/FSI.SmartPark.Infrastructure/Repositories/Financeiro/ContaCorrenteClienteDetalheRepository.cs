using FSI.SmartPark.Domain.Entities.Financeiro;
using FSI.SmartPark.Domain.Interfaces.Financeiro;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Financeiro;

public class ContaCorrenteClienteDetalheRepository : RepositoryBase<ContaCorrenteClienteDetalhe>, IContaCorrenteClienteDetalheRepository
{
    protected override string Tabela => "ContaCorrenteClienteDetalhe";
    public ContaCorrenteClienteDetalheRepository(SmartParkDbContext ctx) : base(ctx) { }
}
