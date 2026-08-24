using SmartPark.Domain.Entities;
using SmartPark.Domain.ValueObjects;
namespace SmartPark.Domain.Repositories
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        Task<Vehicle?> GetByPlateAsync(Guid companyId, Plate plate, CancellationToken cancellationToken = default);
        Task<IEnumerable<Vehicle>> GetPagedByCustomerAsync(Guid companyId, Guid customerId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}