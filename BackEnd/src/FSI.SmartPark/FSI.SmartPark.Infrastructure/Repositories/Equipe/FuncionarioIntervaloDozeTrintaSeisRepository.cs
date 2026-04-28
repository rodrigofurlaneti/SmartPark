using FSI.SmartPark.Domain.Entities.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Equipe;

public class FuncionarioIntervaloDozeTrintaSeisRepository : RepositoryBase<FuncionarioIntervaloDozeTrintaSeis>, IFuncionarioIntervaloDozeTrintaSeisRepository
{
    protected override string Tabela => "FuncionarioIntervaloDozeTrintaSeis";
    public FuncionarioIntervaloDozeTrintaSeisRepository(SmartParkDbContext ctx) : base(ctx) { }
}
