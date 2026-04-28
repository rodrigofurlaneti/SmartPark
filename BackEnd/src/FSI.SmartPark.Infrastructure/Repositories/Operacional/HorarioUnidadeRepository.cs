using FSI.SmartPark.Domain.Entities.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Operacional;

public class HorarioUnidadeRepository : RepositoryBase<HorarioUnidade>, IHorarioUnidadeRepository
{
    protected override string Tabela => "HorarioUnidade";
    public HorarioUnidadeRepository(SmartParkDbContext ctx) : base(ctx) { }
}
