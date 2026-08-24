using SmartPark.Domain.Entities;
using SmartPark.Domain.ValueObjects;
namespace SmartPark.Domain.Repositories
{
    public interface IPersonRepository : IRepository<Person>
    {
        Task<Person?> GetByTaxIdAsync(TaxId taxId, CancellationToken cancellationToken = default);
    }
}