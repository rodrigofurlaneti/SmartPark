using FSI.SmartPark.Domain.Entities.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Operacional;

public class TabelaPrecoAvulsoRepository : RepositoryBase<TabelaPrecoAvulso>, ITabelaPrecoAvulsoRepository
{
    protected override string Tabela => "TabelaPrecoAvulso";
    public TabelaPrecoAvulsoRepository(SmartParkDbContext ctx) : base(ctx) { }
}
