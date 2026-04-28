using FSI.SmartPark.Domain.Entities.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Comercial;

public class VagaCortesiaVigenciaRepository : RepositoryBase<VagaCortesiaVigencia>, IVagaCortesiaVigenciaRepository
{
    protected override string Tabela => "VagaCortesiaVigencia";
    public VagaCortesiaVigenciaRepository(SmartParkDbContext ctx) : base(ctx) { }
}
