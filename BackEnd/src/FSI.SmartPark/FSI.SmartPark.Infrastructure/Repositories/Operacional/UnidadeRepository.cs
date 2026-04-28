using FSI.SmartPark.Domain.Entities.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Operacional;

public class UnidadeRepository : RepositoryBase<Unidade>, IUnidadeRepository
{
    protected override string Tabela => "Unidade";
    public UnidadeRepository(SmartParkDbContext ctx) : base(ctx) { }
}
