using FSI.SmartPark.Domain.Entities.Comercial;
using FSI.SmartPark.Domain.Interfaces.Comercial;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Comercial;

public class VagaCortesiaRepository : RepositoryBase<VagaCortesia>, IVagaCortesiaRepository
{
    protected override string Tabela => "VagaCortesia";
    public VagaCortesiaRepository(SmartParkDbContext ctx) : base(ctx) { }
}
