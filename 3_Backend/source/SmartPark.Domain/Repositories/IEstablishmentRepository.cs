using SmartPark.Domain.Entities;
using SmartPark.Domain.Enums;
namespace SmartPark.Domain.Repositories
{
    public interface IEstablishmentRepository : IRepository<Establishment>
    {
        Task<IEnumerable<Establishment>> GetPagedByCompanyAsync(
            Guid companyId,
            GeneralStatus? status,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
    }
}
