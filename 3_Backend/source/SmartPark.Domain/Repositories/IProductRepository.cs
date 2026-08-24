using SmartPark.Domain.Entities;
namespace SmartPark.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetBySkuAsync(Guid companyId, string sku, CancellationToken cancellationToken = default);
    }
}