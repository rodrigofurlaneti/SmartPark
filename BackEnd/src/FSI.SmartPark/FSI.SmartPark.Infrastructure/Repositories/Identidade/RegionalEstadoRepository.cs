using FSI.SmartPark.Domain.Entities.Identidade;
using FSI.SmartPark.Domain.Interfaces.Identidade;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Identidade;

public class RegionalEstadoRepository : RepositoryBase<RegionalEstado>, IRegionalEstadoRepository
{
    protected override string Tabela => "RegionalEstado";
    public RegionalEstadoRepository(SmartParkDbContext ctx) : base(ctx) { }
}
