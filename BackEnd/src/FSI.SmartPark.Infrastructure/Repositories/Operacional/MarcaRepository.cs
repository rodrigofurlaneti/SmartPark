using FSI.SmartPark.Domain.Entities.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Operacional;

public class MarcaRepository : RepositoryBase<Marca>, IMarcaRepository
{
    protected override string Tabela => "Marca";
    public MarcaRepository(SmartParkDbContext ctx) : base(ctx) { }
}
