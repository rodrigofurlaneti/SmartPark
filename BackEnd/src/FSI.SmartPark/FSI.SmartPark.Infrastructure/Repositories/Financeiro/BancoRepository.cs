using FSI.SmartPark.Domain.Entities.Financeiro;
using FSI.SmartPark.Domain.Interfaces.Financeiro;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Financeiro;

public class BancoRepository : RepositoryBase<Banco>, IBancoRepository
{
    protected override string Tabela => "Banco";
    public BancoRepository(SmartParkDbContext ctx) : base(ctx) { }
}
