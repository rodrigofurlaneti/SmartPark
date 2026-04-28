using FSI.SmartPark.Domain.Entities.Identidade;
using FSI.SmartPark.Domain.Interfaces.Identidade;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Identidade;

public class PessoaJuridicaRepository : RepositoryBase<PessoaJuridica>, IPessoaJuridicaRepository
{
    protected override string Tabela => "PessoaJuridica";
    public PessoaJuridicaRepository(SmartParkDbContext ctx) : base(ctx) { }
}
