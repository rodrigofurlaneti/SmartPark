using SmartPark.Domain.Entities;
namespace SmartPark.Domain.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Task<Invoice?> GetByInvoiceNumberAsync(Guid companyId, string invoiceNumber, CancellationToken cancellationToken = default);
        Task<IEnumerable<Invoice>> GetPagedByCustomerAsync(Guid companyId, Guid customerId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}