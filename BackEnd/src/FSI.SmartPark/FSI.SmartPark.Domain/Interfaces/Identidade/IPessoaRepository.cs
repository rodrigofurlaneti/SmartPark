using FSI.SmartPark.Domain.Entities.Identidade;
namespace FSI.SmartPark.Domain.Interfaces.Identidade
{
    public interface IPessoaRepository : IRepositoryBase<Pessoa>
    {
        Task<Pessoa> ObterPorNome(string nome);
    }
}
