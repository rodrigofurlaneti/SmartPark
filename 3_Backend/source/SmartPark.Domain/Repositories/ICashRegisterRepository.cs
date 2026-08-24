using SmartPark.Domain.Entities;
using SmartPark.Domain.Enums;
namespace SmartPark.Domain.Repositories
{
    public interface ICashRegisterRepository : IRepository<CashRegister>
    {
        Task<CashRegister?> GetOpenRegisterByUserAsync(Guid companyId, Guid parkingId, Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<CashRegister>> GetPagedByParkingAsync(Guid companyId, Guid parkingId, CashRegisterStatus? status, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}
