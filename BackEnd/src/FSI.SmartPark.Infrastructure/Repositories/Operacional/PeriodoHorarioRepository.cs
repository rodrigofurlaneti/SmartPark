using FSI.SmartPark.Domain.Entities.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Operacional;

public class PeriodoHorarioRepository : RepositoryBase<PeriodoHorario>, IPeriodoHorarioRepository
{
    protected override string Tabela => "PeriodoHorario";
    public PeriodoHorarioRepository(SmartParkDbContext ctx) : base(ctx) { }
}
