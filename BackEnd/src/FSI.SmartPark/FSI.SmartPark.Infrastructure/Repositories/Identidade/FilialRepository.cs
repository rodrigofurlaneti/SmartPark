using FSI.SmartPark.Domain.Entities.Identidade;
using FSI.SmartPark.Domain.Interfaces.Identidade;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Identidade;

public class FilialRepository : RepositoryBase<Filial>, IFilialRepository
{
    protected override string Tabela => "Filial";
    public FilialRepository(SmartParkDbContext ctx) : base(ctx) { }
}
