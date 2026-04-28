using FSI.SmartPark.Application.DTOs.Financeiro;
using FSI.SmartPark.Application.Interfaces.Financeiro;
using FSI.SmartPark.Domain.Entities.Financeiro;
using FSI.SmartPark.Domain.Enums;
using FSI.SmartPark.Domain.Interfaces.Financeiro;

namespace FSI.SmartPark.Application.Services.Financeiro;

public class ContasAPagarService : IContasAPagarService
{
    private readonly IContasAPagarRepository _repo;

    public ContasAPagarService(IContasAPagarRepository repo) => _repo = repo;

    public async Task<ContasAPagarResponseDto> Criar(ContasAPagarRequestDto dto)
    {
        var conta  = new ContasAPagar(dto.NumeroDocumento, dto.DataVencimento, dto.ValorTotal, dto.EmpresaId);
        var id     = await _repo.Add(conta);
        var criada = await _repo.GetById(id);
        return ToDto(criada!);
    }

    public async Task<ContasAPagarResponseDto?> ObterPorId(int id)
    {
        var c = await _repo.GetById(id);
        return c is null ? null : ToDto(c);
    }

    public async Task<IEnumerable<ContasAPagarResponseDto>> ListarPorUnidade(int unidadeId)
    {
        var lista = await _repo.GetAll();
        return lista.Where(c => c.Unidade_Id == unidadeId).Select(ToDto);
    }

    public async Task<IEnumerable<ContasAPagarResponseDto>> ListarEmAberto()
    {
        var lista = await _repo.GetAll();
        return lista.Where(c => c.StatusConta == (int)StatusContasAPagar.Aberto).Select(ToDto);
    }

    public async Task Pagar(int id)
    {
        var c = await _repo.GetById(id)
            ?? throw new KeyNotFoundException($"Conta a pagar {id} não encontrada.");
        c.Pagar();
        await _repo.Update(c);
    }

    private static ContasAPagarResponseDto ToDto(ContasAPagar c) => new(
        c.Id,
        c.NumeroDocumento,
        c.DataVencimento,
        c.ValorTotal,
        (StatusContasAPagar)c.StatusConta,
        c.Fornecedor_Id,
        c.Unidade_Id);
}
