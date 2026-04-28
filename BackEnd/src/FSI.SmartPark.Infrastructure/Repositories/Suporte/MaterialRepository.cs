using FSI.SmartPark.Domain.Entities.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Suporte;

public class MaterialRepository : RepositoryBase<Material>, IMaterialRepository
{
    protected override string Tabela => "Material";
    public MaterialRepository(SmartParkDbContext ctx) : base(ctx) { }
}
