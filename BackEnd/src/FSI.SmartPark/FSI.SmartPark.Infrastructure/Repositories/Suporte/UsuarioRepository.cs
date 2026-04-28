using FSI.SmartPark.Domain.Entities.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Suporte;

public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
{
    protected override string Tabela => "Usuario";
    public UsuarioRepository(SmartParkDbContext ctx) : base(ctx) { }
}
