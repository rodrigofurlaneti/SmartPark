using FSI.SmartPark.Domain.Entities.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Comercial;

public class ClienteRepository : RepositoryBase<Cliente>, IClienteRepository
{
    protected override string Tabela => "Cliente";
    public ClienteRepository(SmartParkDbContext ctx) : base(ctx) { }
}
