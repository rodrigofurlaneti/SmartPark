using FSI.SmartPark.Domain.Entities.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Suporte;

public class MenuRepository : RepositoryBase<Menu>, IMenuRepository
{
    protected override string Tabela => "Menu";
    public MenuRepository(SmartParkDbContext ctx) : base(ctx) { }
}
