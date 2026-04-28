using FSI.SmartPark.Domain.Entities.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Comercial;

public class ClienteCondominoRepository : RepositoryBase<ClienteCondomino>, IClienteCondominoRepository
{
    protected override string Tabela => "ClienteCondomino";
    public ClienteCondominoRepository(SmartParkDbContext ctx) : base(ctx) { }
}
