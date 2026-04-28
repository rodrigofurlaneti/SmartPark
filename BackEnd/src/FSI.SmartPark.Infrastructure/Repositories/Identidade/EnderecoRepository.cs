using FSI.SmartPark.Domain.Entities.Identidade;
using FSI.SmartPark.Domain.Interfaces.Identidade;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Identidade;

public class EnderecoRepository : RepositoryBase<Endereco>, IEnderecoRepository
{
    protected override string Tabela => "Endereco";
    public EnderecoRepository(SmartParkDbContext ctx) : base(ctx) { }
}
