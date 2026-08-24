using SmartPark.Domain.Entities;
namespace SmartPark.Domain.Repositories
{
    public interface IStockItemRepository : IRepository<StockItem>
    {
        Task<StockItem?> GetByProductAndParkingAsync(Guid companyId, Guid productId, Guid? parkingId, CancellationToken cancellationToken = default);
    }
}