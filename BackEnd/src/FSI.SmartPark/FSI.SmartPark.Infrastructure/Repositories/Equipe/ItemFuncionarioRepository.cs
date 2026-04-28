using FSI.SmartPark.Domain.Entities.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Equipe;

public class ItemFuncionarioRepository : RepositoryBase<ItemFuncionario>, IItemFuncionarioRepository
{
    protected override string Tabela => "ItemFuncionario";
    public ItemFuncionarioRepository(SmartParkDbContext ctx) : base(ctx) { }
}
