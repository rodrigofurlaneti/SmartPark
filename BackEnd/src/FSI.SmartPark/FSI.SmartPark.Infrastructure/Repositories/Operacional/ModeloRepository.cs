using FSI.SmartPark.Domain.Entities.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Operacional;

public class ModeloRepository : RepositoryBase<Modelo>, IModeloRepository
{
    protected override string Tabela => "Modelo";
    public ModeloRepository(SmartParkDbContext ctx) : base(ctx) { }
}
