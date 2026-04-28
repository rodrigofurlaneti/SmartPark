using FSI.SmartPark.Domain.Entities.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Suporte;

public class EstoqueRepository : RepositoryBase<Estoque>, IEstoqueRepository
{
    protected override string Tabela => "Estoque";
    public EstoqueRepository(SmartParkDbContext ctx) : base(ctx) { }
}
