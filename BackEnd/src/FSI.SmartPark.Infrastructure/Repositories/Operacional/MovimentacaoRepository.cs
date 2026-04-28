using Dapper;
using FSI.SmartPark.Domain.Entities.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Operacional;

public class MovimentacaoRepository : RepositoryBase<Movimentacao>, IMovimentacaoRepository
{
    protected override string Tabela => "Movimentacao";

    public MovimentacaoRepository(SmartParkDbContext ctx) : base(ctx) { }

    public async Task<Movimentacao> ObterAtivoPorPlaca(string placa, int unidadeId)
    {
        using var conn = _ctx.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Movimentacao>(
            "SELECT * FROM Movimentacao WHERE Placa = @placa AND Unidade_Id = @unidadeId AND DataSaida IS NULL",
            new { placa, unidadeId })
            ?? throw new KeyNotFoundException($"Movimentação ativa não encontrada para placa {placa}.");
    }

    public async Task<bool> RegistrarSaida(int id, decimal valorCobrado)
    {
        using var conn = _ctx.CreateConnection();
        var rows = await conn.ExecuteAsync(
            "UPDATE Movimentacao SET DataSaida = @now, ValorCobrado = @valor WHERE Id = @id",
            new { now = DateTime.Now, valor = valorCobrado, id });
        return rows > 0;
    }

    public async Task<IEnumerable<Movimentacao>> ObterPorPeriodo(int unidadeId, DateTime inicio, DateTime fim)
    {
        using var conn = _ctx.CreateConnection();
        return await conn.QueryAsync<Movimentacao>(
            "SELECT * FROM Movimentacao WHERE Unidade_Id = @unidadeId AND DataEntrada BETWEEN @inicio AND @fim",
            new { unidadeId, inicio, fim });
    }
}
