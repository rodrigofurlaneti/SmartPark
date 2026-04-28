using FSI.SmartPark.Domain.Entities.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Suporte;

public class EstoqueMaterialRepository : RepositoryBase<EstoqueMaterial>, IEstoqueMaterialRepository
{
    protected override string Tabela => "EstoqueMaterial";
    public EstoqueMaterialRepository(SmartParkDbContext ctx) : base(ctx) { }
}
