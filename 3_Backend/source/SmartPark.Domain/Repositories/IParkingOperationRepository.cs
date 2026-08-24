using SmartPark.Domain.Entities;
using SmartPark.Domain.Enums;
namespace SmartPark.Domain.Repositories
{
    public interface IParkingOperationRepository : IRepository<ParkingOperation>
    {
        Task<ParkingOperation?> GetActiveByVehicleIdAsync(Guid companyId, Guid vehicleId, CancellationToken cancellationToken = default);
        Task<ParkingOperation?> GetByTicketIdAsync(Guid companyId, Guid ticketId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ParkingOperation>> GetPagedByParkingAndStatusAsync(Guid companyId, Guid parkingId, OperationStatus status, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}