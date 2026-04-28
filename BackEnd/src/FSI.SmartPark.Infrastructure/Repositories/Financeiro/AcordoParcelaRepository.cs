using FSI.SmartPark.Domain.Entities.Financeiro;
using FSI.SmartPark.Domain.Interfaces.Financeiro;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Financeiro;

public class AcordoParcelaRepository : RepositoryBase<AcordoParcela>, IAcordoParcelaRepository
{
    protected override string Tabela => "AcordoParcela";
    public AcordoParcelaRepository(SmartParkDbContext ctx) : base(ctx) { }
}
