using FSI.SmartPark.Domain.Entities.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Comercial;

public class SeloRepository : RepositoryBase<Selo>, ISeloRepository
{
    protected override string Tabela => "Selo";
    public SeloRepository(SmartParkDbContext ctx) : base(ctx) { }
}
