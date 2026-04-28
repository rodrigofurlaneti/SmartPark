using FSI.SmartPark.Domain.Entities.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Equipe;

public class ControlePontoRepository : RepositoryBase<ControlePonto>, IControlePontoRepository
{
    protected override string Tabela => "ControlePonto";
    public ControlePontoRepository(SmartParkDbContext ctx) : base(ctx) { }
}
