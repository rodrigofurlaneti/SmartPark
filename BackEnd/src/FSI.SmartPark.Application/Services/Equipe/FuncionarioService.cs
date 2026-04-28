using FSI.SmartPark.Application.DTOs.Equipe;
using FSI.SmartPark.Application.Interfaces.Equipe;
using FSI.SmartPark.Domain.Entities.Equipe;
using FSI.SmartPark.Domain.Interfaces.Equipe;

namespace FSI.SmartPark.Application.Services.Equipe;

public class FuncionarioService : IFuncionarioService
{
    private readonly IFuncionarioRepository _repo;

    public FuncionarioService(IFuncionarioRepository repo) => _repo = repo;

    public async Task<FuncionarioResponseDto> Criar(FuncionarioRequestDto dto)
    {
        var func = new Funcionario(dto.PessoaId, dto.Salario, dto.Escala);
        var id = await _repo.Inserir(func);
        var criado = await _repo.ObterPorId(id);
        return ToDto(criado!);
    }

    public async Task<FuncionarioResponseDto?> ObterPorId(int id)
    {
        var f = await _repo.ObterPorId(id);
        return f is null ? null : ToDto(f);
    }

    public async Task<IEnumerable<FuncionarioResponseDto>> ListarAtivos()
    {
        var lista = await _repo.ListarTodos();
        return lista.Where(f => f.Status == Domain.Enums.StatusFuncionario.Ativo).Select(ToDto);
    }

    public async Task<IEnumerable<FuncionarioResponseDto>> ListarPorUnidade(int unidadeId)
    {
        var lista = await _repo.ListarTodos();
        return lista.Where(f => f.Unidade_Id == unidadeId).Select(ToDto);
    }

    public async Task AlterarSalario(int id, decimal novoSalario)
    {
        var f = await _repo.ObterPorId(id)
            ?? throw new KeyNotFoundException($"Funcionário {id} não encontrado.");
        f.AlterarSalario(novoSalario);
        await _repo.Atualizar(f);
    }

    public async Task Desligar(int id)
    {
        var f = await _repo.ObterPorId(id)
            ?? throw new KeyNotFoundException($"Funcionário {id} não encontrado.");
        f.Desligar();
        await _repo.Atualizar(f);
    }

    private static FuncionarioResponseDto ToDto(Funcionario f) => new(
        f.Id, f.Pessoa_Id, f.Codigo, f.Salario, f.Status,
        f.TipoEscala, f.DataAdmissao, f.Cargo_Id, f.Unidade_Id);
}
