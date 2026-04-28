using FSI.SmartPark.Domain.Entities.Financeiro;
using FSI.SmartPark.Domain.Interfaces.Financeiro;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Financeiro;

public class AcordoRepository : RepositoryBase<Acordo>, IAcordoRepository
{
    protected override string Tabela => "Acordo";
    public AcordoRepository(SmartParkDbContext ctx) : base(ctx) { }
}
