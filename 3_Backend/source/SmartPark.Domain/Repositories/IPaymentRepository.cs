using SmartPark.Domain.Entities;
using SmartPark.Domain.Enums;
namespace SmartPark.Domain.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetPagedByDateRangeAsync(Guid companyId, DateTime startDate, DateTime endDate, PaymentStatus? status, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}