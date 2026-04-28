using FSI.SmartPark.Domain.Entities.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Equipe;

public class ColaboradorRepository : RepositoryBase<Colaborador>, IColaboradorRepository
{
    protected override string Tabela => "Colaborador";
    public ColaboradorRepository(SmartParkDbContext ctx) : base(ctx) { }
}
