using FSI.SmartPark.Domain.Entities.Identidade;
using FSI.SmartPark.Domain.Interfaces.Identidade;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Identidade;

public class CidadeRepository : RepositoryBase<Cidade>, ICidadeRepository
{
    protected override string Tabela => "Cidade";
    public CidadeRepository(SmartParkDbContext ctx) : base(ctx) { }
}
