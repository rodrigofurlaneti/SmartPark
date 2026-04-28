using FSI.SmartPark.Domain.Entities.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Equipe;

public class BeneficioFuncionarioDetalheRepository : RepositoryBase<BeneficioFuncionarioDetalhe>, IBeneficioFuncionarioDetalheRepository
{
    protected override string Tabela => "BeneficioFuncionarioDetalhe";
    public BeneficioFuncionarioDetalheRepository(SmartParkDbContext ctx) : base(ctx) { }
}
