using SmartPark.Domain.Entities;
namespace SmartPark.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(Guid companyId, string email, CancellationToken cancellationToken = default);
    }
}