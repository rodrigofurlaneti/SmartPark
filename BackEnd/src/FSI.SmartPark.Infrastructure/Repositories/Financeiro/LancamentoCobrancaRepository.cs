using FSI.SmartPark.Domain.Entities.Financeiro;
using FSI.SmartPark.Domain.Interfaces.Financeiro;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Financeiro;

public class LancamentoCobrancaRepository : RepositoryBase<LancamentoCobranca>, ILancamentoCobrancaRepository
{
    protected override string Tabela => "LancamentoCobranca";
    public LancamentoCobrancaRepository(SmartParkDbContext ctx) : base(ctx) { }
}
