using FSI.SmartPark.Domain.Entities.Identidade;
using FSI.SmartPark.Domain.Interfaces.Identidade;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Identidade;

public class RegionalRepository : RepositoryBase<Regional>, IRegionalRepository
{
    protected override string Tabela => "Regional";
    public RegionalRepository(SmartParkDbContext ctx) : base(ctx) { }
}
