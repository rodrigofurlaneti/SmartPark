using FSI.SmartPark.Domain.Entities.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Operacional;

public class TabelaPrecoAvulsoHoraValorRepository : RepositoryBase<TabelaPrecoAvulsoHoraValor>, ITabelaPrecoAvulsoHoraValorRepository
{
    protected override string Tabela => "TabelaPrecoAvulsoHoraValor";
    public TabelaPrecoAvulsoHoraValorRepository(SmartParkDbContext ctx) : base(ctx) { }
}
