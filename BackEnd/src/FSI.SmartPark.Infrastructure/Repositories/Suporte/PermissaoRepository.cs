using FSI.SmartPark.Domain.Entities.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Suporte;

public class PermissaoRepository : RepositoryBase<Permissao>, IPermissaoRepository
{
    protected override string Tabela => "Permissao";
    public PermissaoRepository(SmartParkDbContext ctx) : base(ctx) { }
}
