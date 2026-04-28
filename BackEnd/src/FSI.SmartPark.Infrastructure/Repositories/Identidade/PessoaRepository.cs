using Dapper;
using FSI.SmartPark.Domain.Entities.Identidade;
using FSI.SmartPark.Domain.Interfaces.Identidade;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Identidade;

public class PessoaRepository : RepositoryBase<Pessoa>, IPessoaRepository
{
    protected override string Tabela => "Pessoa";

    public PessoaRepository(SmartParkDbContext ctx) : base(ctx) { }

    public async Task<Pessoa> ObterPorNome(string nome)
    {
        using var conn = _ctx.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Pessoa>(
            "SELECT * FROM Pessoa WHERE Nome LIKE @nome",
            new { nome = $"%{nome}%" })
            ?? throw new KeyNotFoundException($"Pessoa com nome '{nome}' não encontrada.");
    }
}
