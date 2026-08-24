using SmartPark.Domain.Entities;
using SmartPark.Domain.Enums;

namespace SmartPark.Domain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer?> GetByPersonIdAsync(Guid companyId, Guid personId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Customer>> GetPagedByTypeAsync(Guid companyId, CustomerType type, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}