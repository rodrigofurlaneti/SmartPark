using SmartPark.Domain.Entities;
using SmartPark.Domain.Enums;
namespace SmartPark.Domain.Repositories
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<Ticket?> GetByTicketNumberAsync(Guid companyId, Guid parkingId, string ticketNumber, CancellationToken cancellationToken = default);
    }
}