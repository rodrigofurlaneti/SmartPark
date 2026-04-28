using Dapper;
using FSI.SmartPark.Domain.Entities.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Operacional;

public class FaturamentoRepository : RepositoryBase<Faturamento>, IFaturamentoRepository
{
    protected override string Tabela => "Faturamento";

    public FaturamentoRepository(SmartParkDbContext ctx) : base(ctx) { }

    public async Task<IEnumerable<Faturamento>> ObterPorPeriodo(int unidadeId, DateTime inicio, DateTime fim)
    {
        using var conn = _ctx.CreateConnection();
        return await conn.QueryAsync<Faturamento>(
            "SELECT * FROM Faturamento WHERE Unidade_Id = @unidadeId AND DataAbertura BETWEEN @inicio AND @fim",
            new { unidadeId, inicio, fim });
    }
}
