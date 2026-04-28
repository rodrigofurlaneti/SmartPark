using FSI.SmartPark.Domain.Entities.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Comercial;

public class TipoSeloRepository : RepositoryBase<TipoSelo>, ITipoSeloRepository
{
    protected override string Tabela => "TipoSelo";
    public TipoSeloRepository(SmartParkDbContext ctx) : base(ctx) { }
}
