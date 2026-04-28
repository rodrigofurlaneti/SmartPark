using FSI.SmartPark.Domain.Entities.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Operacional;

public class EquipamentoRepository : RepositoryBase<Equipamento>, IEquipamentoRepository
{
    protected override string Tabela => "Equipamento";
    public EquipamentoRepository(SmartParkDbContext ctx) : base(ctx) { }
}
