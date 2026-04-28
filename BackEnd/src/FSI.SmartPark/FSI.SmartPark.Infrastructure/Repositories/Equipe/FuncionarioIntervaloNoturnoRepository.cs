using FSI.SmartPark.Domain.Entities.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Equipe;

public class FuncionarioIntervaloNoturnoRepository : RepositoryBase<FuncionarioIntervaloNoturno>, IFuncionarioIntervaloNoturnoRepository
{
    protected override string Tabela => "FuncionarioIntervaloNoturno";
    public FuncionarioIntervaloNoturnoRepository(SmartParkDbContext ctx) : base(ctx) { }
}
