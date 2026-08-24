using SmartPark.Domain.Entities;
namespace SmartPark.Domain.Repositories
{
    public interface IParkingRepository : IRepository<Parking>
    {
        Task<Parking?> GetByCodeAsync(Guid companyId, string code, CancellationToken cancellationToken = default);
        Task<IEnumerable<Parking>> GetPagedByCompanyAsync(Guid companyId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}