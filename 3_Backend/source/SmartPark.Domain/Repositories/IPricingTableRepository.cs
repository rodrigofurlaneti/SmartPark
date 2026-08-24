using SmartPark.Domain.Entities;
using SmartPark.Domain.Enums;
namespace SmartPark.Domain.Repositories
{
    public interface IPricingTableRepository : IRepository<PricingTable>
    {
        Task<PricingTable?> GetActivePricingTableAsync(
            Guid companyId,
            Guid parkingId,
            OperationType operationType,
            DateTime targetDate,
            CancellationToken cancellationToken = default);
    }
}