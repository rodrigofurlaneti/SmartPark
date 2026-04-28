using FSI.SmartPark.Domain.Entities.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Equipe;

public class FuncionarioIntervaloCompensacaoRepository : RepositoryBase<FuncionarioIntervaloCompensacao>, IFuncionarioIntervaloCompensacaoRepository
{
    protected override string Tabela => "FuncionarioIntervaloCompensacao";
    public FuncionarioIntervaloCompensacaoRepository(SmartParkDbContext ctx) : base(ctx) { }
}
