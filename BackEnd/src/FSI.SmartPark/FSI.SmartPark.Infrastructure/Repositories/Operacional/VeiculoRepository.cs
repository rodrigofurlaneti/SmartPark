using FSI.SmartPark.Domain.Entities.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Operacional;

public class VeiculoRepository : RepositoryBase<Veiculo>, IVeiculoRepository
{
    protected override string Tabela => "Veiculo";
    public VeiculoRepository(SmartParkDbContext ctx) : base(ctx) { }
}
