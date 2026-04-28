using FSI.SmartPark.Domain.Entities.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Comercial;

public class PedidoSeloRepository : RepositoryBase<PedidoSelo>, IPedidoSeloRepository
{
    protected override string Tabela => "PedidoSelo";
    public PedidoSeloRepository(SmartParkDbContext ctx) : base(ctx) { }
}
