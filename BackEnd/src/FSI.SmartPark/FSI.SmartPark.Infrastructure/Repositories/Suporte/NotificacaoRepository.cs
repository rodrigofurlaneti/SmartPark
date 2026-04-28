using FSI.SmartPark.Domain.Entities.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;
using FSI.SmartPark.Infrastructure.Data;

namespace FSI.SmartPark.Infrastructure.Repositories.Suporte;

public class NotificacaoRepository : RepositoryBase<Notificacao>, INotificacaoRepository
{
    protected override string Tabela => "Notificacao";
    public NotificacaoRepository(SmartParkDbContext ctx) : base(ctx) { }
}
