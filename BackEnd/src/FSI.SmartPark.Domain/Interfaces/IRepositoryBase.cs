namespace FSI.SmartPark.Domain.Interfaces
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task<int> Inserir(TEntity entity);
        Task<bool> Atualizar(TEntity entity);
        Task<bool> Excluir(int id);
        Task<TEntity> ObterPorId(int id);
        Task<IEnumerable<TEntity>> ListarTodos();
    }
}