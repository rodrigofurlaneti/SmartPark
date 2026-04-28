using FSI.SmartPark.Application.DTOs.Suporte;
using FSI.SmartPark.Application.Interfaces.Suporte;
using FSI.SmartPark.Domain.Entities.Suporte;
using FSI.SmartPark.Domain.Interfaces.Suporte;

namespace FSI.SmartPark.Application.Services.Suporte;

public class EstoqueService : IEstoqueService
{
    private readonly IEstoqueRepository _estoqueRepo;
    private readonly IEstoqueMaterialRepository _materialRepo;

    public EstoqueService(IEstoqueRepository estoqueRepo, IEstoqueMaterialRepository materialRepo)
    {
        _estoqueRepo = estoqueRepo;
        _materialRepo = materialRepo;
    }

    public async Task<EstoqueResponseDto> Criar(EstoqueRequestDto dto)
    {
        var estoque = new Estoque();
        var id = await _estoqueRepo.Inserir(estoque);
        var criado = await _estoqueRepo.ObterPorId(id);
        return new EstoqueResponseDto(criado!.Id, criado.Nome, criado.EstoquePrincipal, criado.Unidade_Id);
    }

    public async Task<IEnumerable<EstoqueResponseDto>> ListarPorUnidade(int unidadeId)
    {
        var lista = await _estoqueRepo.ListarTodos();
        return lista
            .Where(e => e.Unidade_Id == unidadeId)
            .Select(e => new EstoqueResponseDto(e.Id, e.Nome, e.EstoquePrincipal, e.Unidade_Id));
    }

    public async Task<IEnumerable<EstoqueMaterialResponseDto>> ListarMateriais(int estoqueId)
    {
        var materiais = await _materialRepo.ListarTodos();
        return materiais
            .Where(m => m.Estoque_Id == estoqueId)
            .Select(m => new EstoqueMaterialResponseDto(
                m.Estoque_Id, m.Material_Id, "—", m.Quantidade, 0, false));
    }

    public async Task AdicionarMaterial(int estoqueId, int materialId, int quantidade)
    {
        var todos = await _materialRepo.ListarTodos();
        var item = todos.FirstOrDefault(m => m.Estoque_Id == estoqueId && m.Material_Id == materialId);
        if (item is not null)
        {
            item.AdicionarQuantidade(quantidade);
            await _materialRepo.Atualizar(item);
        }
        else
        {
            var novo = new EstoqueMaterial();
            novo.AdicionarQuantidade(quantidade);
            await _materialRepo.Inserir(novo);
        }
    }

    public async Task RemoverMaterial(int estoqueId, int materialId, int quantidade)
    {
        var todos = await _materialRepo.ListarTodos();
        var item = todos.FirstOrDefault(m => m.Estoque_Id == estoqueId && m.Material_Id == materialId)
            ?? throw new KeyNotFoundException("Material não encontrado no estoque.");
        item.RemoverQuantidade(quantidade);
        await _materialRepo.Atualizar(item);
    }
}
