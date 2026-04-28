using FSI.SmartPark.Domain.Entities.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Suporte;

public class AuditRepository : RepositoryBase<Audit>, IAuditRepository
{
    protected override string Tabela => "Audit";
    public AuditRepository(SmartParkDbContext ctx) : base(ctx) { }
}
