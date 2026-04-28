using FSI.SmartPark.Domain.Interfaces.Equipe;
using FSI.SmartPark.Infrastructure.Data;
using EquipeEntity = FSI.SmartPark.Domain.Entities.Equipe.Equipe;

namespace FSI.SmartPark.Infrastructure.Repositories.Equipe;

public class EquipeRepository : RepositoryBase<EquipeEntity>, IEquipeRepository
{
    protected override string Tabela => "Equipe";
    public EquipeRepository(SmartParkDbContext ctx) : base(ctx) { }
}
