using FSI.SmartPark.Domain.Entities.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Suporte;

public class PerfilRepository : RepositoryBase<Perfil>, IPerfilRepository
{
    protected override string Tabela => "Perfil";
    public PerfilRepository(SmartParkDbContext ctx) : base(ctx) { }
}
