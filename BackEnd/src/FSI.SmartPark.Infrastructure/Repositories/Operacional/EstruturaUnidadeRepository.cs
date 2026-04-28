using FSI.SmartPark.Domain.Entities.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Operacional;

public class EstruturaUnidadeRepository : RepositoryBase<EstruturaUnidade>, IEstruturaUnidadeRepository
{
    protected override string Tabela => "EstruturaUnidade";
    public EstruturaUnidadeRepository(SmartParkDbContext ctx) : base(ctx) { }
}
