using FSI.SmartPark.Domain.Entities.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Comercial;

public class ConvenioRepository : RepositoryBase<Convenio>, IConvenioRepository
{
    protected override string Tabela => "Convenio";
    public ConvenioRepository(SmartParkDbContext ctx) : base(ctx) { }
}
