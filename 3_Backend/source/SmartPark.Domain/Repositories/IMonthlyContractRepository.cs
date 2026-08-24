using SmartPark.Domain.Entities;
using SmartPark.Domain.Enums;
namespace SmartPark.Domain.Repositories
{
    public interface IMonthlyContractRepository : IRepository<MonthlyContract>
    {
        Task<MonthlyContract?> GetByContractNumberAsync(Guid companyId, string contractNumber, CancellationToken cancellationToken = default);
        Task<MonthlyContract?> GetActiveContractByVehicleAsync(Guid companyId, Guid parkingId, Guid vehicleId, DateTime date, CancellationToken cancellationToken = default);
    }
}