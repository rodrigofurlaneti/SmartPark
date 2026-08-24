using SmartPark.Domain.Entities;
namespace SmartPark.Domain.Repositories
{
    public interface IPostpaidAccountRepository : IRepository<PostpaidAccount>
    {
        Task<PostpaidAccount?> GetByAccountNumberAsync(Guid companyId, string accountNumber, CancellationToken cancellationToken = default);
        Task<PostpaidAccount?> GetByCustomerIdAsync(Guid companyId, Guid customerId, CancellationToken cancellationToken = default);
    }
}