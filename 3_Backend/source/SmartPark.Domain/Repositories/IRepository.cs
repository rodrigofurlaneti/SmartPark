using SmartPark.Domain.Primitives;
namespace SmartPark.Domain.Repositories
{
    public interface IRepository<T> where T : AggregateRoot
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        void Update(T entity);
        void Remove(T entity);
    }
}