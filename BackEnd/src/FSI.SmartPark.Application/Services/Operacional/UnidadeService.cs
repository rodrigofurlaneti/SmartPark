using FSI.SmartPark.Application.DTOs.Operacional;
using FSI.SmartPark.Application.Interfaces.Operacional;
using FSI.SmartPark.Domain.Entities.Operacional;
using FSI.SmartPark.Domain.Interfaces.Operacional;

namespace FSI.SmartPark.Application.Services.Operacional;

public class UnidadeService : IUnidadeService
{
    private readonly IUnidadeRepository _repo;

    public UnidadeService(IUnidadeRepository repo) => _repo = repo;

    public async Task<UnidadeResponseDto> Criar(UnidadeRequestDto dto)
    {
        var unidade = new Unidade(dto.Nome, dto.NumeroVagas, dto.DiaVencimento, dto.EmpresaId);
        if (dto.CNPJ is not null) unidade.DefinirCNPJ(dto.CNPJ);
        var id = await _repo.Inserir(unidade);
        var criada = await _repo.ObterPorId(id);
        return ToDto(criada!);
    }

    public async Task<UnidadeResponseDto> Atualizar(int id, UnidadeRequestDto dto)
    {
        var unidade = await _repo.ObterPorId(id)
            ?? throw new KeyNotFoundException($"Unidade {id} não encontrada.");
        unidade.AtualizarCapacidade(dto.NumeroVagas);
        await _repo.Atualizar(unidade);
        return ToDto(unidade);
    }

    public async Task<UnidadeResponseDto?> ObterPorId(int id)
    {
        var u = await _repo.ObterPorId(id);
        return u is null ? null : ToDto(u);
    }

    public async Task<IEnumerable<UnidadeResponseDto>> ListarAtivas()
    {
        var lista = await _repo.ListarTodos();
        return lista.Where(u => u.Ativa).Select(ToDto);
    }

    public async Task<IEnumerable<UnidadeResponseDto>> ListarTodas()
    {
        var lista = await _repo.ListarTodos();
        return lista.Select(ToDto);
    }

    public async Task Inativar(int id)
    {
        var u = await _repo.ObterPorId(id)
            ?? throw new KeyNotFoundException($"Unidade {id} não encontrada.");
        u.Inativar();
        await _repo.Atualizar(u);
    }

    private static UnidadeResponseDto ToDto(Unidade u) => new(
        u.Id, u.Codigo, u.Nome, u.NumeroVaga, u.Ativa,
        u.DiaVencimento, u.CNPJ, u.Empresa_Id, u.Funcionario_Id, u.Endereco_Id);
}
