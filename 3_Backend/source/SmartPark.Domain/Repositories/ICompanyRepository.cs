using SmartPark.Domain.Entities;
namespace SmartPark.Domain.Repositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Company?> GetByTaxIdAsync(string taxId, CancellationToken cancellationToken = default);
        Task<bool> ExistsByTaxIdAsync(string taxId, CancellationToken cancellationToken = default);
    }
}