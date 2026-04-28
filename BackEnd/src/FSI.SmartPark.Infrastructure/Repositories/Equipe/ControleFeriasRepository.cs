using FSI.SmartPark.Domain.Entities.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Equipe;

public class ControleFeriasRepository : RepositoryBase<ControleFerias>, IControleFeriasRepository
{
    protected override string Tabela => "ControleFerias";
    public ControleFeriasRepository(SmartParkDbContext ctx) : base(ctx) { }
}
