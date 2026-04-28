using FSI.SmartPark.Domain.Entities.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Comercial;

public class ContratoMensalistaRepository : RepositoryBase<ContratoMensalista>, IContratoMensalistaRepository
{
    protected override string Tabela => "ContratoMensalista";
    public ContratoMensalistaRepository(SmartParkDbContext ctx) : base(ctx) { }
}
