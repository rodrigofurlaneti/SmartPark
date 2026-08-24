using SmartPark.Domain.Entities;
using SmartPark.Domain.Enums;
namespace SmartPark.Domain.Repositories
{
    public interface IWorkOrderRepository : IRepository<WorkOrder>
    {
        Task<WorkOrder?> GetByOrderNumberAsync(Guid companyId, string orderNumber, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkOrder>> GetPagedByVehicleAsync(Guid companyId, Guid vehicleId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}