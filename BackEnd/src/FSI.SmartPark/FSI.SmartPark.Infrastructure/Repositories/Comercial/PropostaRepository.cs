using FSI.SmartPark.Domain.Entities.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Comercial;

public class PropostaRepository : RepositoryBase<Proposta>, IPropostaRepository
{
    protected override string Tabela => "Proposta";
    public PropostaRepository(SmartParkDbContext ctx) : base(ctx) { }
}
