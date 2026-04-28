using FSI.SmartPark.Domain.Entities.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Equipe;

public class TipoBeneficioRepository : RepositoryBase<TipoBeneficio>, ITipoBeneficioRepository
{
    protected override string Tabela => "TipoBeneficio";
    public TipoBeneficioRepository(SmartParkDbContext ctx) : base(ctx) { }
}
