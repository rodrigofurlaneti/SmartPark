using FSI.SmartPark.Domain.Entities.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Equipe;

public class FuncionarioRepository : RepositoryBase<Funcionario>, IFuncionarioRepository
{
    protected override string Tabela => "Funcionario";
    public FuncionarioRepository(SmartParkDbContext ctx) : base(ctx) { }
}
